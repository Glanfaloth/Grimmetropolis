using Microsoft.Xna.Framework;

using System;
using System.Diagnostics;

public class CatapultAnimation : EntityAnimation
{
    public EnemyCatapult Catapult;

    public TDTransform Body;
    public TDTransform FrontWheel;
    public TDTransform BackWheel;
    public TDTransform Arm;
    public TDTransform ReloadWheel;

    private TDMesh _bodyMesh;
    private TDMesh _frontWheelMesh;
    private TDMesh _backWheelMesh;
    private TDMesh _armMesh;
    private TDMesh _reloadWheel;

    private Vector3 _bodyPositionStandard = new Vector3(0f, 0f, 0f);
    private Vector3 _frontWheelPositionStandard = new Vector3(.475f, 0f, .175f);
    private Vector3 _backWheelPositionStandard = new Vector3(-.475f, 0f, .175f);
    private Vector3 _armPositionStandard = new Vector3(0f, 0f, .34f);
    private Vector3 _reloadWheelPositionStandard = new Vector3(-.68f, 0f, .39f);

    private Quaternion _armRotationStandard = Quaternion.CreateFromAxisAngle(Vector3.Up, 0f);
    private Quaternion _armRotationShooting = Quaternion.CreateFromAxisAngle(Vector3.Up, .8f * MathHelper.PiOver2);

    private float _wheelAngle = 0f;
    private float _rotationReload = 2f* MathHelper.TwoPi;

    private bool _armInUse = false;
    public float PartialArmUseTime = .3f;

    public override void Initialize()
    {
        base.Initialize();

        Catapult = Character as EnemyCatapult;

        CreateBodyPart("Body", out Body, out _bodyMesh);
        CreateBodyPart("FrontWheel", out FrontWheel, out _frontWheelMesh);
        CreateBodyPart("BackWheel", out BackWheel, out _backWheelMesh);
        CreateBodyPart("Arm", out Arm, out _armMesh);
        CreateBodyPart("ReloadWheel", out ReloadWheel, out _reloadWheel);

        Body.LocalPosition = _bodyPositionStandard;
        FrontWheel.LocalPosition = _frontWheelPositionStandard;
        BackWheel.LocalPosition = _backWheelPositionStandard;
        Arm.LocalPosition = _armPositionStandard;
        ReloadWheel.LocalPosition = _reloadWheelPositionStandard;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (Character.CurrentWalkSpeed <= 1e-5f) IdleAnimation(gameTime);
        else WalkAnimation(gameTime, Character.CurrentWalkSpeed);
    }

    protected override void IdleAnimation(GameTime gameTime) { }

    protected override void WalkAnimation(GameTime gameTime, float speed)
    {
        _wheelAngle = (_wheelAngle + .2f * speed / MathHelper.TwoPi) % MathHelper.TwoPi;

        FrontWheel.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Up, _wheelAngle);
        BackWheel.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Up, _wheelAngle);
    }

    public override  void UseAnimation()
    {
        if (_armInUse) return;

        _armInUse = true;

        Quaternion currentRightArmRotation = Arm.LocalRotation;
        Arm.TDObject.RunAction(PartialArmUseTime, (p) =>
        {
            Arm.LocalRotation = Quaternion.Lerp(currentRightArmRotation, _armRotationShooting, p);
        }, () =>
        {
            Arm.TDObject.RunAction(4f * PartialArmUseTime, (p) =>
            {
                Arm.LocalRotation = Quaternion.Lerp(_armRotationShooting, _armRotationStandard, MathF.Pow(p, 2f));
                ReloadWheel.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, _rotationReload);
            }, 2f * PartialArmUseTime, _armInUse = false);
        });
    }

    public override void Highlight(bool highlight)
    {
        _bodyMesh.Highlight(highlight);
        _bodyMesh.Highlight(highlight);
        _frontWheelMesh.Highlight(highlight);
        _backWheelMesh.Highlight(highlight);
        _armMesh.Highlight(highlight);
        _reloadWheel.Highlight(highlight);
    }
}