using Microsoft.Xna.Framework;

public class Castle : Building
{
    public override float BuildTime => 0;

    public MagicalArtifact MagicalArtifact = null;

    private bool _stealPossible = false;

    public override float Health
    {
        get => _health;
        set
        {
            if (value <= 1f)
            {
                value = 1f;
                _stealPossible = true;
            }
            else _stealPossible = false;
            base.Health = value;
        }
    }

    public override void Initialize()
    {
        Size.X = Config.CASTLE_SIZE_X;
        Size.Y = Config.CASTLE_SIZE_Y;

        BaseHealth = Config.CASTLE_HEALTH;
        Health = Config.CASTLE_HEALTH;

        // TODO: remove this work around
        // this needs to be passable since the ai tries to go to the castle location, otherwise no path will be found
        IsPassable = false;

        MagicalArtifact = PrefabFactory.CreatePrefab(PrefabType.MagicalArtifact, GameManager.Instance.ItemTransform).GetComponent<MagicalArtifact>();
        MagicalArtifact.Structure = this;

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

    public void StealMagicalArtifact(Character character)
    {
        if (_stealPossible && MagicalArtifact != null)
        {
            MagicalArtifact.Structure = null;
            MagicalArtifact.TakeItem(character);
            MagicalArtifact = null;
        }
    }

    public void ReceiveMagicalArtifact(MagicalArtifact magicalArtifact)
    {
        if (MagicalArtifact == null)
        {
            MagicalArtifact = magicalArtifact;
            magicalArtifact.PlaceAtBuilding(this);
        }
    }
}
