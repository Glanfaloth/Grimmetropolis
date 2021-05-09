using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class ResourceBuilding : Building
{
    public override ResourcePile GetResourceCost() => new ResourcePile(Config.RESOURCE_BUILDING_WOOD_COST, Config.RESOURCE_BUILDING_STONE_COST);
    public override float BuildTime => Config.RESOURCE_BUILDING_BUILD_VALUE;

    public TDCylinderCollider InteractionCollider;

    private float _time = 1f;
    private float _collectionTime = 1f;
    private List<ResourceDeposit> _interactingResourceDeposits = new List<ResourceDeposit>();

    public override void Initialize()
    {
        Health = Config.RESOURCE_BUILDING_HEALTH;
        BaseHealth = Config.RESOURCE_BUILDING_HEALTH;
        _time = Config.RESOURCE_BUILDING_COLLECTION_TIME;
        _collectionTime = Config.RESOURCE_BUILDING_COLLECTION_TIME;

        InteractionCollider.IsTrigger = true;
        InteractionCollider.Radius = Config.RESOURCE_BUILDING_COLLECTION_RANGE;
        InteractionCollider.Height = 2f;
        InteractionCollider.Offset = Vector3.Zero;
        InteractionCollider.collisionEvent += GetClosestCollider;

        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!IsPreview && !IsBlueprint)
        {
            _time -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_time < 0f)
            {
                foreach (ResourceDeposit resourceDeposit in _interactingResourceDeposits)
                {
                    resourceDeposit.HarvestResource();
                }
                _time += _collectionTime;
            }
        }

        _interactingResourceDeposits.Clear();
    }

    public override void Destroy()
    {
        InteractionCollider.collisionEvent -= GetClosestCollider;
        InteractionCollider = null;

        base.Destroy();
    }

    private void GetClosestCollider(TDCollider collider1, TDCollider collider2, float intersection)
    {
        TDCollider oppositeCollider = InteractionCollider == collider2 ? collider1 : collider2;
        ResourceDeposit resourceDeposit = oppositeCollider.TDObject.GetComponent<MapTile>()?.Structure as ResourceDeposit;
        if (resourceDeposit != null)
        {
            _interactingResourceDeposits.Add(resourceDeposit);
        }
    }
}