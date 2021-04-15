using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

public static class Config
{
    #region PathingAI

    public static readonly float RUN_MOVE_DIAGONAL_BASE_COST = (float)Math.Sqrt(2);
    public const float RUN_MOVE_DIRECT_BASE_COST = 1;
    public const float RUN_MOVE_DISTANCE_FACTOR = 2;
    public const float RUN_MOVE_OUTPOST_COUNT_SCALE_FACTOR = 0.5f;
    // RUN_MOVE_OUTPOST_COUNT_SCALE_FACTOR = linear
    public const float ATTACK_MOVE_COST = 0.5f;
    public const float RANGED_ATTACK_MOVE_BASE_COST = .25f;
    public const float RANGED_ATTACK_MOVE_DISTANCE_FACTOR = 0f;


    public const int ENEMY_OUTPOST_AVOIDANCE_RANGE = 3;
    // ENEMY_OUTPOST_AVOIDANCE_RANGE_FUNCTION = manhattan distance

    // TODO: find if there is a better way to do this
    public const int MAX_RANGED_ATTACK = 10;


    public const float WAVE_ALERT_TIME = 8;
    public const float TIME_UNTIL_FIRST_WAVE = 10;
    public const float TIME_BETWEEN_WAVES = 20;
    public const float FIRST_WAVE_WITCH_COUNT = 4;
    public const float FIRST_WAVE_KNIGHT_COUNT = 6;

    // this results in first siege appearing in 5th wave
    public const float FIRST_WAVE_SIEGE_COUNT = .41f;
    public const float WAVE_GROWTH_FACTOR = 1.25f;

    public const int WAVE_SPAWN_RADIUS = 5;

    #endregion

    #region Buildings

    public const float BUILDING_DEFAULT_HEALTH = 3f;

    public const int CASTLE_SIZE_X = 3;
    public const int CASTLE_SIZE_Y = 3;
    public const float CASTLE_HEALTH = 9f;


    public const float OUTPOST_HEALTH = 3f;
    public const int OUTPOST_WOOD_COST = 1;
    public const int OUTPOST_STONE_COST = 1;
    public const float OUTPOST_SHOOTING_RANGE = 3f;
    public const float OUTPOST_SHOOTING_RATE = .5f;

    public const float OUTPOST_ARROW_DAMAGE = 1f;
    public const float OUTPOST_ARROW_SPEED = 6f;

    public const float OUTPOST_BUILD_VALUE = 2f;

    public const int WALL_WOOD_COST = 0;
    public const int WALL_STONE_COST = 2;
    public const float WALL_HEALTH = 9f;

    public const int RESOURCE_WOOD_GATHER_BASE_RATE = 1;
    public const int RESOURCE_STONE_GATHER_BASE_RATE = 1;

    public const float RESOURCE_WOOD_GATHER_DURATION = .25f;
    public const float RESOURCE_STONE_GATHER_DURATION = .25f;

    #endregion

    #region Entities

    // TODO: Enemy move types
    public const float PLAYER_WALK_SPEED = 4f;
    public const float PLAYER_ROTATE_SPEED = 3f * MathHelper.Pi;
    public const float PLAYER_HEALTH = 3f;
    public const float PLAYER_DAMAGE = 1f;
    public const float PLAYER_ATTACK_DURATION = 1f;

    public const float PLAYER_PLACE_BUILDING_COOLDOWN = .5f;
    public const float PLAYER_BUILD_COOLDONW = .5f;
    public const float PLAYER_BUILD_STRENGTH = 1f;

    public const float ENEMY_PROJECTILE_SPEED = 6f;

    public static readonly EnemyStats ENEMY_CATAPULT_STATS = new EnemyStats(1f, 1f * MathHelper.Pi, 5f, 1f, 2f, 4f, 8f);
    public static readonly EnemyStats ENEMY_WITCH_STATS = new EnemyStats(3f, 3f * MathHelper.Pi, 1f, 1f, 1f, 2f, 4f);
    public static readonly EnemyStats ENEMY_KNIGHTS_STATS = new EnemyStats(3f, 3f * MathHelper.Pi, 3f, 1f, 1f, .25f, 2f);

    #endregion

    public struct EnemyStats
    {
        public readonly float WALK_SPEED;
        public readonly float ROTATE_SPEED;
        public readonly float HEALTH;
        public readonly float DAMAGE_AGAINST_PLAYER;
        public readonly float DAMAGE_AGAINST_BUILDINGS;
        public readonly float ATTACK_RANGE;
        public readonly float ATTACK_DURATION;

        public EnemyStats(float walkSpeed, float rotateSpeed, float health, float damageAgainstPlayer, float damageAgainstBuilding, float attackRange, float attackDuration)
        {
            WALK_SPEED = walkSpeed;
            ROTATE_SPEED = rotateSpeed;
            HEALTH = health;
            DAMAGE_AGAINST_PLAYER = damageAgainstPlayer;
            DAMAGE_AGAINST_BUILDINGS = damageAgainstBuilding;
            ATTACK_RANGE = attackRange;
            ATTACK_DURATION = attackDuration;
        }
    }
}
