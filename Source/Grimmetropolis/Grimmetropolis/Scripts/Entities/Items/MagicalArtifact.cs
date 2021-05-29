using Microsoft.Xna.Framework;

public class MagicalArtifact : Item
{

    public Structure Structure = null;

    public override void Initialize()
    {
        base.Initialize();

        CarryPosition = new Vector3(0f, 0f, .4f);
        CarryRotation = Quaternion.Identity;
        CarryScale = .75f * Vector3.One;
    }
    public override void TakeItem(Character character)
    {
        base.TakeItem(character);

        if (character.Animation is CharacterAnimation characterAnimation) TDObject.Transform.Parent = characterAnimation.Head;
        else TDObject.Transform.Parent = character.TDObject.Transform;
        TDObject.Transform.LocalPosition = CarryPosition;
        TDObject.Transform.LocalRotation = CarryRotation;
        TDObject.Transform.LocalScale = CarryScale;

        if (character.Animation == null)
        {
            TDObject.Transform.LocalPosition = new Vector3(0f, 0f, .8f);
        }
    }

    protected override void SetMapTransform()
    {
        if (Structure != null) Position = Structure.Position + new Point((int)(.5f * Structure.Size.X), (int)(.5f * Structure.Size.Y));

        base.SetMapTransform();

        if (Structure != null)
        {
            Vector3 position = TDObject.Transform.Position;
            position.Z = 3f;
            TDObject.Transform.Position = position;
        }
    }

    public override void InteractWithStructure(GameTime gameTime, Structure structure)
    {
        if (structure is Castle castle)
        {
            castle.ReceiveMagicalArtifact(this);
        }
    }
}
