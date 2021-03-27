using Microsoft.Xna.Framework;

public class TestComponent : TDComponent
{

    private Vector3 _eulerAngles;
    private float _speedAngles;

    public override void Initialize()
    {
        base.Initialize();

        _eulerAngles = Vector3.Zero;
        _speedAngles = .5f;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        foreach (TDInput input in TDInputManager.Inputs)
        {
            Vector2 j1Direction = input.GetMoveDirection();
            _eulerAngles.X += j1Direction.X * _speedAngles * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _eulerAngles.Y += j1Direction.Y * _speedAngles * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsSelectBuildingTypePressed()) _eulerAngles.Z += _speedAngles * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsSpecialAbilityPressed()) _eulerAngles.Z -= _speedAngles * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        TDObject.Transform.LocalRotation = Quaternion.CreateFromYawPitchRoll(_eulerAngles.X, _eulerAngles.Y, _eulerAngles.Z);
    }
}
