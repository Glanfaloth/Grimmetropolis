using Microsoft.Xna.Framework;

public class Item : TDComponent
{
    public Point Position = Point.Zero;
    public Character Character = null;

    public TDMesh Mesh;

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
        }
        SetMapTransform();
        PlaceItem(this, null);
    }

    public void TakeItem(Character character)
    {
        character.Items[0] = this;
        TDObject.Transform.Parent = character.TDObject.Transform;
        TDObject.Transform.LocalPosition = new Vector3(0f, -.3f, .55f);
        TDObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Forward, MathHelper.PiOver2) * Quaternion.CreateFromAxisAngle(Vector3.Down, MathHelper.PiOver2);
        PlaceItem(null, this);
    }

    private void SetMapTransform()
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
}
