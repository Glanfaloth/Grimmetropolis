using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class Hospital : Building
{
    public override ResourcePile GetResourceCost() => new ResourcePile(Config.HOSPITAL_WOOD_COST, Config.HOSPITAL_STONE_COST);
    public override float BuildTime => Config.HOSPITAL_BUILD_VALUE;

    public TDCylinderCollider InteractionCollider;

    private float _healRate = 1f;
    private List<Player> _interactingPlayers = new List<Player>();

    public override void Initialize()
    {
        Health = Config.HOSPITAL_HEALTH;
        BaseHealth = Config.HOSPITAL_HEALTH;
        _healRate = Config.HOSPITAL_HEAL_RATE;

        InteractionCollider.IsTrigger = true;
        InteractionCollider.Radius = Config.HOSPTIAL_HEAL_RANGE;
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
            foreach (Player player in _interactingPlayers)
            {
                player.Health += (float)gameTime.ElapsedGameTime.TotalSeconds / _healRate;
            }
        }

        _interactingPlayers.Clear();
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
        Player player = oppositeCollider.TDObject.GetComponent<Player>();
        if (player != null)
        {
            _interactingPlayers.Add(player);
        }
    }
}