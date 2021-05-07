using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Farm : Building
{
    public override float BuildTime => Config.FARM_BUILD_VALUE;
    public override ResourcePile GetResourceCost() => new ResourcePile(Config.FARM_WOOD_COST, Config.FARM_STONE_COST);
    public override ResourcePile GetResourceUpkeep() => new ResourcePile(0, 0, Config.FARM_FOOD_UPKEEP);

    public Point MillPosition = new Point(0, 1);

    private TDTransform _millRotorTransform;

    private TDMesh _millRotorMesh;

    public override void Initialize()
    {
        Size = new Point(2, 2);

        CreateBuildingPart(Mesh.Model, Mesh.Texture, "MillRotor", out _millRotorTransform, out _millRotorMesh);
        CreateMainBuildingPart(Mesh.Model, Mesh.Texture, "Farm");

        _millRotorTransform.LocalPosition = new Vector3(.1f, .9f, 1f);
        _millRotorTransform.TDObject.RunAction(4f, (p) =>
        {
            _millRotorTransform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Normalize(new Vector3(2f, -1f, 0f)), MathHelper.TwoPi * p);
        }, true);

        base.Initialize();
    }

    protected override void SetAsPreview()
    {
        base.SetAsPreview();
        _millRotorMesh.IsPreview = true;
        _millRotorMesh.BaseColor = Mesh.BaseColor;
    }

    protected override void SetAsBlueprint()
    {
        base.SetAsBlueprint();
        _millRotorMesh.IsPreview = true;
        _millRotorMesh.BaseColor = Mesh.BaseColor;
    }

    public override bool TryBuild(float buildStrength)
    {
        if (_buildProgress + buildStrength >= BuildTime)
        {
            _millRotorMesh.IsPreview = false;
            _millRotorMesh.BaseColor = Vector4.One;
        }

        return base.TryBuild(buildStrength);
    }

    public bool GetMillCollider(MapTile mapTile)
    {
        return mapTile.Position - Position - MillPosition == Point.Zero;
    }

    public override void Highlight(bool highlight)
    {
        base.Highlight(highlight);
        _millRotorMesh.Highlight(highlight);
    }
}
