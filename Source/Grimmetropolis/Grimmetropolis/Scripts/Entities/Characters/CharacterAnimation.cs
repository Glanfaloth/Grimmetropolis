using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Diagnostics;

public class CharacterAnimation : TDComponent
{
    public Character Character;

    public TDTransform Head;
    public TDTransform Body;
    public TDTransform LeftLeg;
    public TDTransform RightLeg;
    public TDTransform LeftArm;
    public TDTransform RightArm;

    public Model CharacterModel;

    private TDMesh _headMesh;
    private TDMesh _bodyMesh;
    private TDMesh _leftLegMesh;
    private TDMesh _rightLegMesh;
    private TDMesh _leftArmMesh;
    private TDMesh _rightArmMesh;

    private Vector3 _headPositionStandard = new Vector3(0f, 0f, .6f);
    private Vector3 _headPositionElevated = new Vector3(0f, 0f, .625f);
    private Vector3 _headPositionHigh = new Vector3(0f, 0f, .65f);
    private Vector3 _bodyPositionStandard = new Vector3(0f, 0f, .5f);
    private Vector3 _bodyPositionElevated = new Vector3(0f, 0f, .525f);
    private Vector3 _bodyPositionHigh = new Vector3(0f, 0f, .55f);
    private Vector3 _leftLegPositionStandard = new Vector3(0f, .06f, .3f);
    private Vector3 _leftLegPositionHigh = new Vector3(0f, .06f, .35f);
    private Vector3 _rightLegPositionStandard = new Vector3(0f, -.06f, .3f);
    private Vector3 _rightLegPositionHigh = new Vector3(0f, -.06f, .35f);
    private Vector3 _leftArmPositionStandard = new Vector3(0f, .2f, .6f);
    private Vector3 _leftArmPositionElevated = new Vector3(0f, .2f, .625f);
    private Vector3 _leftArmPositionHigh = new Vector3(0f, .2f, .65f);
    private Vector3 _rightArmPositionStandard = new Vector3(0f, -.2f, .6f);
    private Vector3 _rightArmPositionElevated = new Vector3(0f, -.2f, .625f);
    private Vector3 _rightArmPositionHigh = new Vector3(0f, -.2f, .65f);

    private Quaternion _legForward = Quaternion.CreateFromAxisAngle(Vector3.Up, -MathHelper.PiOver4);
    private Quaternion _legBackward = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.PiOver4);
    private Quaternion _armForward = Quaternion.CreateFromAxisAngle(Vector3.Up, -MathHelper.Pi / 6f);
    private Quaternion _armBackward = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.Pi / 6f);

    private float _time = 0f;

    private bool _isWalking = false;
    private bool _readyForAnimation = true;
    private float _resetTime = .1f;

    public override void Initialize()
    {
        base.Initialize();

        CreateBodyPart("Head", out Head, out _headMesh);
        CreateBodyPart("Body", out Body, out _bodyMesh);
        CreateBodyPart("LeftLeg", out LeftLeg, out _leftLegMesh);
        CreateBodyPart("RightLeg", out RightLeg, out _rightLegMesh);
        CreateBodyPart("LeftArm", out LeftArm, out _leftArmMesh);
        CreateBodyPart("RightArm", out RightArm, out _rightArmMesh);

        Head.LocalPosition = _headPositionStandard;
        Body.LocalPosition = _bodyPositionStandard;
        LeftLeg.LocalPosition = _leftLegPositionStandard;
        RightLeg.LocalPosition = _rightLegPositionStandard;
        LeftArm.LocalPosition = _leftArmPositionStandard;
        RightArm.LocalPosition = _rightArmPositionStandard;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!_readyForAnimation) return;

        if (Character.CurrentWalkSpeed <= 1e-5f) IdleAnimation(gameTime);
        else WalkAnimation(gameTime, Character.CurrentWalkSpeed);
    }

    private void CreateBodyPart(string bodyPart, out TDTransform bodyPartTransform, out TDMesh bodyPartMesh)
    {
        TDObject bodyPartObject = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        bodyPartMesh = bodyPartObject.AddComponent<TDMesh>();

        ModelBone bone; CharacterModel.Bones.TryGetValue(bodyPart, out bone);
        ModelMesh mesh; CharacterModel.Meshes.TryGetValue(bodyPart, out mesh);

        bodyPartMesh.Model = new Model(TDSceneManager.Graphics.GraphicsDevice, new List<ModelBone>() { bone }, new List<ModelMesh>() { mesh });
        bodyPartMesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");

        bodyPartTransform = bodyPartObject.Transform;
    }

    private void ResetAnimation()
    {
        _readyForAnimation = false;

        ResetBodyPart(Head, _headPositionStandard, Quaternion.Identity);
        ResetBodyPart(Body, _bodyPositionStandard, Quaternion.Identity);
        ResetBodyPart(LeftLeg, _leftLegPositionStandard, Quaternion.Identity);
        ResetBodyPart(RightLeg, _rightLegPositionStandard, Quaternion.Identity);
        ResetBodyPart(LeftArm, _leftArmPositionStandard, Quaternion.Identity);
        ResetBodyPart(RightArm, _rightArmPositionStandard, Quaternion.Identity);

        TDObject.RemoveActions();
        TDObject.RunAction(_resetTime, (p) => { }, () =>
        {
            _readyForAnimation = true;
            _time = 0f;
        });
    }

    private void ResetBodyPart(TDTransform transform, Vector3 targetPosition, Quaternion targetRotation)
    {
        transform.TDObject.RemoveActions();

        Vector3 currentPosition = transform.LocalPosition;
        Quaternion currentRotation = transform.LocalRotation;
        transform.TDObject.RunAction(_resetTime, (p) =>
        {
            transform.LocalPosition = Vector3.Lerp(currentPosition, targetPosition, p);
            transform.LocalRotation = Quaternion.Lerp(currentRotation, targetRotation, p);
        });
    }

    private void IdleAnimation(GameTime gameTime)
    {
        if (_isWalking)
        {
            ResetAnimation();
            _isWalking = false;
            return;
        }

        _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        float idleProgress = MathF.Sin(_time % 4f * MathHelper.PiOver2);

        Head.LocalPosition = Vector3.Lerp(_headPositionStandard, _headPositionElevated, idleProgress);
        Body.LocalPosition = Vector3.Lerp(_bodyPositionStandard, _bodyPositionElevated, idleProgress);
        LeftArm.LocalPosition = Vector3.Lerp(_leftArmPositionStandard, _leftArmPositionElevated, idleProgress);
        RightArm.LocalPosition = Vector3.Lerp(_rightArmPositionStandard, _rightArmPositionElevated, idleProgress);
    }

    private void WalkAnimation(GameTime gameTime, float speed)
    {
        if (!_isWalking)
        {
            ResetAnimation();
            _isWalking = true;
            return;
        }

        _time += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        float walkElevationProgress = MathF.Sin(_time % 1f * MathHelper.TwoPi);
        float walkLimbsProgress = .5f * MathF.Sin(_time % 2f * MathHelper.Pi) + .5f;

        Head.LocalPosition = Vector3.Lerp(_headPositionStandard, _headPositionHigh, walkElevationProgress);
        Body.LocalPosition = Vector3.Lerp(_bodyPositionStandard, _bodyPositionHigh, walkElevationProgress);
        LeftArm.LocalPosition = Vector3.Lerp(_leftArmPositionStandard, _leftArmPositionHigh, walkElevationProgress);
        RightArm.LocalPosition = Vector3.Lerp(_rightArmPositionStandard, _rightArmPositionHigh, walkElevationProgress);
        LeftLeg.LocalPosition = Vector3.Lerp(_leftLegPositionStandard, _leftLegPositionHigh, walkElevationProgress);
        RightLeg.LocalPosition = Vector3.Lerp(_rightLegPositionStandard, _rightLegPositionHigh, walkElevationProgress);

        LeftArm.LocalRotation = Quaternion.Lerp(_armForward, _armBackward, walkLimbsProgress);
        RightArm.LocalRotation = Quaternion.Lerp(_armBackward, _armForward, walkLimbsProgress);
        LeftLeg.LocalRotation = Quaternion.Lerp(_legBackward, _legForward, walkLimbsProgress);
        RightLeg.LocalRotation = Quaternion.Lerp(_legForward, _legBackward, walkLimbsProgress);
    }

    public void Highlight(bool highlight)
    {
        _headMesh.Highlight(highlight);
        _bodyMesh.Highlight(highlight);
        _leftLegMesh.Highlight(highlight);
        _rightLegMesh.Highlight(highlight);
        _leftArmMesh.Highlight(highlight);
        _rightArmMesh.Highlight(highlight);
    }
}