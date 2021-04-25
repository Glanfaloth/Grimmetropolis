using Microsoft.Xna.Framework;

public class Item : TDComponent
{
    public Point Position = Point.Zero;
    public Character Character = null;

    public TDMesh Mesh;

    private float _cooldown = .2f;

    protected Vector3 CarryPosition = new Vector3(.05f, -.08f, -.3f);
    protected Quaternion CarryRotation = Quaternion.CreateFromAxisAngle(Vector3.Left, MathHelper.PiOver2);
    protected Vector3 CarryScale = Vector3.One;

    public override void Initialize()
    {
        base.Initialize();

        if (Character == null) Drop();
        else TakeItem(Character);

        GameManager.Instance.Items.Add(this);
    }

    public void Drop()
    {
        if (Character != null)
        {
            Position = GameManager.Instance.Map.GetMapTile(Character.InteractionCollider.CenterXY).Position;
            Character.Items[0] = null;
            Character.Cooldown = _cooldown;
            if (Character is Player player)
            {
                player.SetProgressForCooldown();
            }
        }
        SetMapTransform();
        PlaceItem(this, null);

        Character = null;
    }

    public void PlaceAtBuilding(Structure structure)
    {
        if (this is MagicalArtifact magicalArtifact)
        {
            magicalArtifact.Structure = structure;
        }
        SetMapTransform();

        if (Character != null)
        {
            Character.Items[0] = null;
            Character.Cooldown = _cooldown;
            if (Character is Player player)
            {
                player.SetProgressForCooldown();
            }
        }
        PlaceItem(null, this);

        Character = null;
    }

    public virtual void TakeItem(Character character)
    {

        character.Items[0] = this;
        TDObject.Transform.Parent = character.Animation.RightArm;
        TDObject.Transform.LocalPosition = CarryPosition;
        TDObject.Transform.LocalRotation = CarryRotation;
        TDObject.Transform.LocalScale = CarryScale;
        PlaceItem(null, this);

        Character = character;

        Character.Cooldown = _cooldown;
        if (Character is Player player)
        {
            player.SetProgressForCooldown();
        }
    }

    protected virtual void SetMapTransform()
    {
        TDObject.Transform.Parent = null;

        Vector3 position = GameManager.Instance.Map.MapTiles[Position.X, Position.Y].TDObject.Transform.Position;
        TDObject.Transform.Position = position;
        TDObject.Transform.LocalRotation = Quaternion.Identity;
    }

    private void PlaceItem(Item item, Item previousItem)
    {
        if (GameManager.Instance.Map.MapTiles[Position.X, Position.Y].Item == previousItem)
        {
            GameManager.Instance.Map.MapTiles[Position.X, Position.Y].Item = item;
        }

        // TODO: in case two players place their item at the exact same time: cancel on drop off of item.
    }
    public void Highlight(bool highlight)
    {
        Mesh.Highlight(highlight);
    }

    public virtual void InteractWithCharacter(GameTime gameTime, Character character)
    {

    }

    public virtual void InteractWithStructure(GameTime gameTime, Structure structure)
    {
        if (Character is Player player && structure.Mesh.IsBlueprint)
        {
            player.Build(gameTime);
            player.Animation.UseArm();
        }
    }
}
