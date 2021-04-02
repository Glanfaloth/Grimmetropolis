﻿using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Enemy : Character
{
    private EnemyController _controller;
    private readonly List<EnemyMove.Type> _moves = new List<EnemyMove.Type>() { EnemyMove.Type.Run };

    public override void Initialize()
    {
        base.Initialize();

        _controller = GameManager.Instance.EnemyController;

        Health = 1f;

        GameManager.Instance.Enemies.Add(this);
    }

    public override void Update(GameTime gameTime)
    {
        if (_controller == null) return;

        EnemyMove nextMove = _controller.ComputeNextMove(new Vector2(TDObject.Transform.LocalPosition.X, TDObject.Transform.LocalPosition.Y), _moves);

        switch (nextMove.MovementType)
        {
            case EnemyMove.Type.None:
                Debug.WriteLine("ERROR: no valid move found for enemy");
                break;
            case EnemyMove.Type.StealArtifact:
                // TODO: implement win condition
                break;
            case EnemyMove.Type.Run:
                MoveToTarget((RunMove)nextMove, gameTime);
                break;
            case EnemyMove.Type.Attack:
                AttackTarget((AttackMove)nextMove, gameTime);
                break;
            default:
                throw new NotSupportedException();
        }

        base.Update(gameTime);
    }

    public override void Destroy()
    {
        base.Destroy();

        GameManager.Instance.Enemies.Remove(this);
    }
    private void Interact()
    {
        float closestPlayerDistance = float.MaxValue;
        float closestBuildingDistance = float.MaxValue;
        Player closestPlayer = null;
        Building closestBuilding = null;
        foreach (Tuple<TDCollider, float> colliderEntry in _colliderList)
        {
            if (colliderEntry.Item1 is TDCylinderCollider && closestPlayerDistance > colliderEntry.Item2)
            {
                Player player = colliderEntry.Item1.TDObject?.GetComponent<Player>();
                if (player != null)
                {
                    closestPlayerDistance = colliderEntry.Item2;
                    closestPlayer = player;
                }
            }
            else if (colliderEntry.Item1 is TDCuboidCollider && closestBuildingDistance > colliderEntry.Item2)
            {
                Building structure = colliderEntry.Item1.TDObject?.GetComponent<Building>();
                if (structure != null)
                {
                    closestBuildingDistance = colliderEntry.Item2;
                    closestBuilding = structure;
                }
            }
        }

        if (closestPlayer != null)
        {
            closestPlayer.Health -= 1f;
        }
        else if (closestBuilding != null)
        {
            closestBuilding.Health -= 1f;
        }
    }

    private void AttackTarget(AttackMove nextMove, GameTime gameTime)
    {
        Interact();
    }

    private void MoveToTarget(RunMove runMove, GameTime gameTime)
    {
        Vector2 direction = runMove.Destination - new Vector2(TDObject.Transform.LocalPosition.X, TDObject.Transform.LocalPosition.Y);
        if (direction.LengthSquared() > 1f) direction.Normalize();
        Move(direction, gameTime);
    }
}
