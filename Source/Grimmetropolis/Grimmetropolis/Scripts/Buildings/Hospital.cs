using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class Hospital : Building
{
    public override ResourcePile GetResourceCost() => new ResourcePile(Config.HOSPITAL_WOOD_COST, Config.HOSPITAL_STONE_COST);
    private ResourcePile GetResurrectionCost() => new ResourcePile(Config.RESURRECTION_COST_WOOD, Config.RESURRECTION_COST_STONE, Config.RESURRECTION_COST_FOOD);

    public override float BuildTime => Config.HOSPITAL_BUILD_VALUE;

    public TDCylinderCollider InteractionCollider;

    public ReviveMenu ReviveMenu;

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

    internal override bool InteractWithPlayer(GameTime gameTime, Player player)
    {
        if (!ReviveMenu.IsShowing) return false;

        bool baseResult = base.InteractWithPlayer(gameTime, player);

        List<PlayerInfo> deadPlayers = new List<PlayerInfo>();

        foreach (PlayerInfo info in GameManager.Instance.ActivePlayerInfos)
        {
            if (info.Instance.Health <= 0)
            {
                deadPlayers.Add(info);
            }
        }

        if (deadPlayers.Count > 0 && ResourcePile.CheckAvailability(GameManager.Instance.ResourcePool, GetResurrectionCost()))
        {
            PlayerInfo luckyPlayer = TDRandom.SelectRandomEntry(deadPlayers);
            PrefabFactory.SpawnPlayer(luckyPlayer, player.TDObject.Transform.LocalPosition);
            GameManager.Instance.ResourcePool -= GetResurrectionCost();

            return true;
        }

        return baseResult;
    }
}