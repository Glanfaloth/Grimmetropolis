using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System.Diagnostics;

public class TestComponent : TDComponent
{

    private Vector3 _eulerAngles;
    private float _speedAngles;
    public TestComponent(TDObject tdObject) : base(tdObject)
    {
        _eulerAngles = Vector3.Zero;
        _speedAngles = .5f;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        KeyboardState state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.Q)) _eulerAngles.X += _speedAngles * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (state.IsKeyDown(Keys.A)) _eulerAngles.X -= _speedAngles * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (state.IsKeyDown(Keys.W)) _eulerAngles.Y += _speedAngles * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (state.IsKeyDown(Keys.S)) _eulerAngles.Y -= _speedAngles * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (state.IsKeyDown(Keys.E)) _eulerAngles.Z += _speedAngles * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (state.IsKeyDown(Keys.D)) _eulerAngles.Z -= _speedAngles * (float)gameTime.ElapsedGameTime.TotalSeconds;

        TDObject.Transform.LocalRotation = Quaternion.CreateFromYawPitchRoll(_eulerAngles.X, _eulerAngles.Y, _eulerAngles.Z);
        Debug.WriteLine(_eulerAngles);
    }
}
