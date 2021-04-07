using Microsoft.Xna.Framework;

public class CameraToPlayers : TDComponent
{

    private Vector3 _defaultOffset = new Vector3(8f, 0f, 18f);
    private Quaternion _defaultRotation = Quaternion.CreateFromYawPitchRoll(-.375f * MathHelper.Pi, 0f, MathHelper.Pi);
    private Quaternion _specialRotation = Quaternion.CreateFromYawPitchRoll(-.5f * MathHelper.Pi, 0f, MathHelper.Pi);

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (GameManager.Instance == null) return;

        Vector3 centerPlayer = GameManager.Instance.Players[0].TDObject.Transform.Position;
        Vector3 minPosition = GameManager.Instance.Players[0].TDObject.Transform.Position;
        Vector3 maxPosition = GameManager.Instance.Players[0].TDObject.Transform.Position;
        for (int i = 1; i < GameManager.Instance.Players.Count; i++)
        {
            centerPlayer += GameManager.Instance.Players[i].TDObject.Transform.Position;

            minPosition = Vector3.Min(GameManager.Instance.Players[i].TDObject.Transform.Position, minPosition);
            maxPosition = Vector3.Max(GameManager.Instance.Players[i].TDObject.Transform.Position, maxPosition);
        }
        centerPlayer /= GameManager.Instance.Players.Count;

        float distance = MathHelper.Max(.08f * (maxPosition.X - minPosition.X), .06f * (maxPosition.Y - minPosition.Y));

        TDObject.Transform.Position = centerPlayer + MathHelper.Max(distance, 1f) * _defaultOffset;
        TDObject.Transform.Rotation = Quaternion.Lerp(_defaultRotation, _specialRotation, MathHelper.Min(0.04f * distance, 1f));
    }
}
