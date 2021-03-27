﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class Enemy : Character
{
    private EnemyController _controller;
    private readonly List<EnemyMove.Type> _moves = new List<EnemyMove.Type>() { EnemyMove.Type.Run };

    public override void Initialize()
    {
        base.Initialize();

        _controller = GameManager.Instance.EnemyController;

        GameManager.Instance.Enemies.Add(this);
    }

    public override void Update(GameTime gameTime)
    {
        if (_controller == null) return;

        EnemyMove nextMove = _controller.ComputeNextMove(new Vector2(TDObject.Transform.LocalPosition.X, TDObject.Transform.LocalPosition.Y), _moves);

        switch (nextMove.MovementType)
        {
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
    }
    public override void Destroy()
    {
        base.Destroy();

        GameManager.Instance.Enemies.Remove(this);
    }

    private void AttackTarget(AttackMove nextMove, GameTime gameTime)
    {
        throw new NotImplementedException();
    }

    private void MoveToTarget(RunMove runMove, GameTime gameTime)
    {
        Vector2 direction = runMove.Destination - new Vector2(TDObject.Transform.LocalPosition.X, TDObject.Transform.LocalPosition.Y);
        if (direction.LengthSquared() > 1f) direction.Normalize();
        Move(direction, gameTime);
    }
}
