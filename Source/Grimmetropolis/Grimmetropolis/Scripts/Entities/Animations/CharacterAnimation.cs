using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Diagnostics;

public class CharacterAnimation : EntityAnimation
{
    public TDTransform Head;
    public TDTransform Body;
    public TDTransform LeftLeg;
    public TDTransform RightLeg;
    public TDTransform LeftArm;
    public TDTransform RightArm;

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
    private Quaternion _armStandard = Quaternion.Identity;
    private Quaternion _armUsing = Quaternion.CreateFromAxisAngle(Vector3.Up, -3f * MathHelper.PiOver4);

    private float _time = 0f;

    private bool _isWalking = false;
    private bool _readyForAnimation = true;
    private float _resetTime = .1f;

    private bool _armInUse = false;
    private float _halfArmUsageTime = .15f;

    public override void Initialize()
    {
        base.Initialize();

        CreateBodyParts();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!_readyForAnimation) return;

        if ((Character?.CurrentWalkSpeed ?? 0f) <= 1e-5f) IdleAnimation(gameTime);
        else WalkAnimation(gameTime, Character.CurrentWalkSpeed);
    }

    private void ResetAnimation()
    {
        _readyForAnimation = false;

        ResetBodyPart(Head, _headPositionStandard, Quaternion.Identity);
        ResetBodyPart(Body, _bodyPositionStandard, Quaternion.Identity);
        ResetBodyPart(LeftLeg, _leftLegPositionStandard, Quaternion.Identity);
        ResetBodyPart(RightLeg, _rightLegPositionStandard, Quaternion.Identity);
        ResetBodyPart(LeftArm, _leftArmPositionStandard, Quaternion.Identity);
        if (!_armInUse) ResetBodyPart(RightArm, _rightArmPositionStandard, Quaternion.Identity);

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

    private void ResetUseAnimation()
    {
        if (_isWalking)
        {
            float futureWalkLimbsProgress = CalculateWalkLimbProgress(_time + _resetTime);
            Quaternion currentRightArmRotation = RightArm.LocalRotation;
            Quaternion targetRigthArmRotation = Quaternion.Lerp(_armBackward, _armForward, futureWalkLimbsProgress);
            RightArm.TDObject.RunAction(_resetTime, (p) =>
            {
                RightArm.LocalRotation = Quaternion.Lerp(currentRightArmRotation, targetRigthArmRotation, p);
            }, () =>
                {
                    _armInUse = false;
                });
        }
        else _armInUse = false;
    }

    protected override void IdleAnimation(GameTime gameTime)
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

    protected override void WalkAnimation(GameTime gameTime, float speed)
    {
        if (!_isWalking)
        {
            ResetAnimation();
            _isWalking = true;
            return;
        }

        _time += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        float walkElevationProgress = MathF.Sin(_time % 1f * MathHelper.TwoPi);
        float walkLimbsProgress = CalculateWalkLimbProgress(_time);

        Head.LocalPosition = Vector3.Lerp(_headPositionStandard, _headPositionHigh, walkElevationProgress);
        Body.LocalPosition = Vector3.Lerp(_bodyPositionStandard, _bodyPositionHigh, walkElevationProgress);
        LeftArm.LocalPosition = Vector3.Lerp(_leftArmPositionStandard, _leftArmPositionHigh, walkElevationProgress);
        RightArm.LocalPosition = Vector3.Lerp(_rightArmPositionStandard, _rightArmPositionHigh, walkElevationProgress);
        LeftLeg.LocalPosition = Vector3.Lerp(_leftLegPositionStandard, _leftLegPositionHigh, walkElevationProgress);
        RightLeg.LocalPosition = Vector3.Lerp(_rightLegPositionStandard, _rightLegPositionHigh, walkElevationProgress);

        LeftArm.LocalRotation = Quaternion.Lerp(_armForward, _armBackward, walkLimbsProgress);
        if (!_armInUse) RightArm.LocalRotation = Quaternion.Lerp(_armBackward, _armForward, walkLimbsProgress);
        LeftLeg.LocalRotation = Quaternion.Lerp(_legBackward, _legForward, walkLimbsProgress);
        RightLeg.LocalRotation = Quaternion.Lerp(_legForward, _legBackward, walkLimbsProgress);
    }

    private float CalculateWalkLimbProgress(float time)
    {
        return .5f * MathF.Sin(time % 2f * MathHelper.Pi) + .5f;
    }

    public override  void UseAnimation()
    {
        if (_armInUse) return;

        _armInUse = true;

        Quaternion currentRightArmRotation = RightArm.LocalRotation;
        RightArm.TDObject.RunAction(_halfArmUsageTime, (p) =>
        {
            RightArm.LocalRotation = Quaternion.Lerp(currentRightArmRotation, _armUsing, p * (2f - p));
        }, () =>
        {
            RightArm.TDObject.RunAction(_halfArmUsageTime, (p) =>
            {
                RightArm.LocalRotation = Quaternion.Lerp(_armUsing, _armStandard, MathF.Pow(p, 2f));
            }, () => ResetUseAnimation());
        });
    }

    public override void Highlight(bool highlight)
    {
        _headMesh.Highlight(highlight);
        _bodyMesh.Highlight(highlight);
        _leftLegMesh.Highlight(highlight);
        _rightLegMesh.Highlight(highlight);
        _leftArmMesh.Highlight(highlight);
        _rightArmMesh.Highlight(highlight);
    }

    public override void SetShowing()
    {
        if (_bodyMesh == null) return;

        _headMesh.IsShowing = IsShowing;
        _bodyMesh.IsShowing = IsShowing;
        _leftLegMesh.IsShowing = IsShowing;
        _rightLegMesh.IsShowing = IsShowing;
        _leftArmMesh.IsShowing = IsShowing;
        _rightArmMesh.IsShowing = IsShowing;
    }

    private void CreateBodyParts()
    {
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

    public void RecreateBodyParts()
    {
        Head.TDObject.Destroy();
        Body.TDObject.Destroy();
        LeftLeg.TDObject.Destroy();
        RightLeg.TDObject.Destroy();
        LeftArm.TDObject.Destroy();
        RightArm.TDObject.Destroy();

        CreateBodyParts();
    }

    public Model GetModelFromPlayerType(PlayerType playerType)
    {
        return playerType switch
        {
            PlayerType.Cinderella => TDContentManager.LoadModel("PlayerCindarella"),
            PlayerType.Snowwhite => TDContentManager.LoadModel("PlayerSnowwhite"),
            PlayerType.Frog => TDContentManager.LoadModel("PlayerFrog"),
            PlayerType.Beast => TDContentManager.LoadModel("PlayerBeast"),
            PlayerType.Cat => TDContentManager.LoadModel("PlayerCat"),
            _ => TDContentManager.LoadModel("PlayerCindarella")
        };
    }
}