using Microsoft.Xna.Framework;

public enum PrefabType
{
    Empty,
    Camera,
    Light,
    Default,
    Map,
    Player,
    Enemy
}

public static class PrefabFactory
{
    public static TDObject CreatePrefab(PrefabType type, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, TDTransform parent)
    {
        TDObject prefab = new TDObject(localPosition, localRotation, localScale, parent);

        switch (type)
        {
            case PrefabType.Camera:
                prefab.Components.Add(new TDCamera(prefab, MathHelper.PiOver2, 2f, 20f));
                break;
            case PrefabType.Light:
                prefab.Components.Add(new TDLight(prefab, MathHelper.PiOver2, 2f, 20f));
                break;
            case PrefabType.Default:
                prefab.Components.Add(new TDMesh(prefab, "DefaultModel", "DefaultTexture"));
                break;
            case PrefabType.Map:
                prefab.Components.Add(new Map(prefab));
                break;
            case PrefabType.Player:
                prefab.Components.Add(new TDMesh(prefab, "PlayerCindarella", "ColorPaletteTexture"));
                prefab.Components.Add(new TDCylinderCollider(prefab, false, .5f, 1f, .5f * Vector3.Backward));
                prefab.Components.Add(new Player(prefab, 0f));
                break;
            case PrefabType.Enemy:
                prefab.Components.Add(new TDMesh(prefab, "EnemyWitch", "ColorPaletteTexture"));
                prefab.Components.Add(new TDCylinderCollider(prefab, false, .5f, 1f, .5f * Vector3.Backward));
                prefab.Components.Add(new Enemy(prefab, 0f));
                break;
        }

        return prefab;
    }

    public static TDObject CreatePrefab(PrefabType type)
    {
        return CreatePrefab(type, Vector3.Zero, Quaternion.Identity, Vector3.One, null);
    }

    public static TDObject CreatePrefab(PrefabType type, Vector3 localPosition, Quaternion localRotation)
    {
        return CreatePrefab(type, localPosition, localRotation, Vector3.One, null);
    }

    public static TDObject CreatePrefab(PrefabType type, TDTransform parent)
    {
        return CreatePrefab(type, Vector3.Zero, Quaternion.Identity, Vector3.One, parent);
    }

    public static TDObject CreatePrefab(PrefabType type, Vector3 localPosition, Quaternion localRotation, TDTransform parent)
    {
        return CreatePrefab(type, localPosition, localRotation, Vector3.One, parent);
    }
}
