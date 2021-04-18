using Microsoft.Xna.Framework;

public class Castle : Building
{
    public override float BuildTime => 0;

    public MagicalArtifact MagicalArtifact = null;

    public override void Initialize()
    {
        Size.X = Config.CASTLE_SIZE_X;
        Size.Y = Config.CASTLE_SIZE_Y;

        BaseHealth = Config.CASTLE_HEALTH;
        Health = Config.CASTLE_HEALTH;

        // TODO: remove this work around
        // this needs to be passable since the ai tries to go to the castle location, otherwise no path will be found
        IsPassable = true;

        MagicalArtifact = PrefabFactory.CreatePrefab(PrefabType.MagicalArtifact, GameManager.Instance.ItemTransform).GetComponent<MagicalArtifact>();
        MagicalArtifact.Castle = this;

        base.Initialize();
    }

    public override void Destroy()
    {
        base.Destroy();

        MagicalArtifact?.TDObject.Destroy();
    }

    protected override void SetMapTransform()
    {
        base.SetMapTransform();

        MagicalArtifact.Position = Position + new Point(1, 1);
    }

    public override ResourcePile GetResourceCost()
    {
        return new ResourcePile(0, 0);
    }

    protected override void DoUpdate(GameTime gameTime)
    {
    }
}
