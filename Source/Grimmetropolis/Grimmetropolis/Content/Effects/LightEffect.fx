#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World;
float4x4 ViewProjection;

Texture2D Texture;
sampler TextureSampler = sampler_state
{
	texture = <Texture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = clamp;
	AddressV = clamp;
};

float3 LightPosition;
float4x4 LightViewProjection;

float AmbientIntensity;
float4 AmbientColor;

float DiffuseIntensity;
float4 DiffuseColor;

Texture2D Shadow;
sampler ShadowSampler = sampler_state
{
	texture = <Shadow>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = clamp;
	AddressV = clamp;
};

// Light effect
struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
	float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float3 Normal : TEXCOORD0;
	float2 TexCoord : TEXCOORD1;
	float3 LightDirection : TEXCOORD2;
	float4 PositionFromLight : TEXCOORD3;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 worldPosition = mul(input.Position, World);
	output.Position = mul(worldPosition, ViewProjection);
	output.Normal = normalize(mul(input.Normal.xyz, World));
	output.TexCoord = input.TexCoord;
	output.LightDirection = normalize(worldPosition.xyz - LightPosition);
	output.PositionFromLight = mul(worldPosition, LightViewProjection);

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float2 projectedShadowCoords;
	projectedShadowCoords.x = .5 * input.PositionFromLight.x / input.PositionFromLight.w + .5;
	projectedShadowCoords.y = -.5 * input.PositionFromLight.y / input.PositionFromLight.w + .5;

	bool inShadow = false;
	if ((saturate(projectedShadowCoords.x) == projectedShadowCoords.x) && (saturate(projectedShadowCoords.y) == projectedShadowCoords.y))
	{
		float depthFromShadow = tex2D(ShadowSampler, projectedShadowCoords).x;
		float depth = input.PositionFromLight.z / input.PositionFromLight.w;

		inShadow = depth - 1e-3 > depthFromShadow;
	}

	float normalIntensity = !inShadow * saturate(dot(-input.LightDirection, input.Normal));

	float4 ambient = AmbientIntensity * AmbientColor;
	float4 diffuse = DiffuseIntensity * normalIntensity * DiffuseColor;

	return (ambient + diffuse) * tex2D(TextureSampler, input.TexCoord);
}

technique LightEffect
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};

// Shadow effect
struct VertexShadowInput
{
	float4 Position : POSITION0;
};

struct VertexShadowOutput
{
	float4 Position : SV_POSITION;
	float4 PositionFromLight : TEXCOORD0;
};

VertexShadowOutput ShadowVS(in VertexShadowInput input)
{
	VertexShadowOutput output = (VertexShadowOutput)0;

	output.Position = mul(input.Position, mul(World, LightViewProjection));
	output.PositionFromLight = output.Position;

	return output;
}

float4 ShadowPS(VertexShadowOutput input) : COLOR
{
	float depth = input.PositionFromLight.z / input.PositionFromLight.w;
	return depth;
}

technique ShadowEffect
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL ShadowVS();
		PixelShader = compile PS_SHADERMODEL ShadowPS();
	}
};