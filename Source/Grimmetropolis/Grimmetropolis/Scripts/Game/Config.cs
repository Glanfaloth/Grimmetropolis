﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

public static class Config
{
    #region PathingAI

    public static readonly float RUN_MOVE_DIAGONAL_BASE_COST = (float)Math.Sqrt(2);
    public const float RUN_MOVE_DIRECT_BASE_COST = 1;
    public const float RUN_MOVE_OUTPOST_COUNT_SCALE_FACTOR = 0.5f;
    // RUN_MOVE_OUTPOST_COUNT_SCALE_FACTOR = linear
    public const float ATTACK_MOVE_COST = 0.5f;


    public const int ENEMY_OUTPOST_AVOIDANCE_RANGE = 3;
    // ENEMY_OUTPOST_AVOIDANCE_RANGE_FUNCTION = manhattan distance

    #endregion

    #region Buildings

    public const float BUILDING_DEFAULT_HEALTH = 3f;

    public const int CASTLE_SIZE_X = 3;
    public const int CASTLE_SIZE_Y = 3;
    public const float CASTLE_HEALTH = 9f;

    public const float OUTPOST_HEALTH = 3f;
    public const float OUTPOST_WOOD_COST = 1f;
    public const float OUTPOST_STONE_COST = 1f;
    public const float OUTPOST_SHOOTING_RANGE = 3f;
    public const float OUTPOST_SHOOTING_RATE = .5f;
    public const float OUTPOST_SHOOTING_DAMAGE = 1f;

    public const float RESOURCE_WOOD_GATHER_BASE_RATE = 1f;
    public const float RESOURCE_STONE_GATHER_BASE_RATE = 1f;

    #endregion

    #region Entities

    // TODO: Enemy move types
    public const float PLAYER_WALK_SPEED = 4f;
    public const float PLAYER_ROTATE_SPEED = 3f * MathHelper.Pi;
    public const float PLAYER_HEALTH = 3f;
    public const float PLAYER_DAMAGE = 1f;

    public static readonly EnemyStats ENEMY_WITCH_STATS = new EnemyStats(4f, 3f * MathHelper.Pi, 1f, 1f, 1f);
    public static readonly EnemyStats ENEMY_KNIGHTS_STATS = new EnemyStats(4f, 3f * MathHelper.Pi, 1f, 1f, 1f);

    #endregion

    public struct EnemyStats
    {
        public readonly float WALK_SPEED;
        public readonly float ROTATE_SPEED;
        public readonly float HEALTH;
        public readonly float DAMAGE_AGAINST_PLAYER;
        public readonly float DAMAGE_AGAINST_BUILDINGS;

        public EnemyStats(float walkSpeed, float rotateSpeed, float health, float damageAgainstPlayer, float damageAgainstBuilding)
        {
            WALK_SPEED = walkSpeed;
            ROTATE_SPEED = rotateSpeed;
            HEALTH = health;
            DAMAGE_AGAINST_PLAYER = damageAgainstPlayer;
            DAMAGE_AGAINST_BUILDINGS = damageAgainstBuilding;
        }
    }
}