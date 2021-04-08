using Microsoft.Xna.Framework;

using System;
using System.Diagnostics;

public class Player : Character
{
    public TDInput Input;

    public override float WalkSpeed => Config.PLAYER_WALK_SPEED;

    protected override float RotateSpeed => Config.PLAYER_ROTATE_SPEED;

    public override float BaseHealth => Config.PLAYER_HEALTH;

    public override Vector3 OffsetTarget => .5f * Vector3.Backward;

    public override void Initialize()
    {
        base.Initialize();

        GameManager.Instance.Players.Add(this);
    }

    public override void Update(GameTime gameTime)
    {
        Vector2 inputDirection = Input.GetMoveDirection();
        Move(new Vector2(-inputDirection.Y, inputDirection.X), gameTime);

        if (Input.IsSpecialAbilityPressed()) Interact();
        if (Input.IsUseItemPressed()) Build();

        base.Update(gameTime);
    }

    public override void Destroy()
    {
        base.Destroy();

        Input = null;
        GameManager.Instance.Players.Remove(this);
    }


    private void Interact()
    {
        float closestEnemyDistance = float.MaxValue;
        float closestStructureDistance = float.MaxValue;
        Enemy closestEnemy = null;
        Structure closestStructure = null;
        foreach (Tuple<TDCollider, float> colliderEntry in _colliderList)
        {
            if (colliderEntry.Item1 is TDCylinderCollider && closestEnemyDistance > colliderEntry.Item2)
            {
                Enemy enemy = colliderEntry.Item1.TDObject?.GetComponent<Enemy>();
                if (enemy != null)
                {
                    closestEnemyDistance = colliderEntry.Item2;
                    closestEnemy = enemy;
                }
            }
            else if (colliderEntry.Item1 is TDCuboidCollider && closestStructureDistance > colliderEntry.Item2)
            {
                Structure structure = colliderEntry.Item1.TDObject?.GetComponent<MapTile>().Structure;
                if (structure != null)
                {
                    closestStructureDistance = colliderEntry.Item2;
                    closestStructure = structure;
                }
            }
        }

        if (closestEnemy != null)
        {
            closestEnemy.Health -= Config.PLAYER_DAMAGE;
        }
        else if (closestStructure != null)
        {
            if (closestStructure is ResourceDeposit closestResourceDeposit)
            {
                closestResourceDeposit.HarvestResource();
            }

            // TODO: Left here for testing usage
            if (closestStructure is Building closestBuilding)
            {
                closestBuilding.Health -= Config.PLAYER_DAMAGE;
            }
        }
    }
}
