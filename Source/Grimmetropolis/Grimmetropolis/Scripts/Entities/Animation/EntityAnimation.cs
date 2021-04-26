using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

public abstract class EntityAnimation : TDComponent
{
    public Character Character;

    public Model CharacterModel;
    public Texture2D CharacterTexture;

    protected void CreateBodyPart(string bodyPart, out TDTransform bodyPartTransform, out TDMesh bodyPartMesh)
    {
        TDObject bodyPartObject = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        bodyPartMesh = bodyPartObject.AddComponent<TDMesh>();

        ModelBone bone; CharacterModel.Bones.TryGetValue(bodyPart, out bone);
        ModelMesh mesh; CharacterModel.Meshes.TryGetValue(bodyPart, out mesh);

        bodyPartMesh.Model = new Model(TDSceneManager.Graphics.GraphicsDevice, new List<ModelBone>() { bone }, new List<ModelMesh>() { mesh });
        bodyPartMesh.Texture = CharacterTexture;

        bodyPartTransform = bodyPartObject.Transform;
    }

    protected abstract void IdleAnimation(GameTime gameTime);

    protected abstract void WalkAnimation(GameTime gameTime, float speed);

    public abstract void UseAnimation();

    public abstract void Highlight(bool highlight);
}