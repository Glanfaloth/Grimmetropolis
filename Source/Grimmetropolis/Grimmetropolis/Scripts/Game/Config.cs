using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

public static class Config
{
    #region EnemyBehaviour

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

    public const float WAVE_ALERT_TIME = 20f;
    public const float TIME_UNTIL_FIRST_WAVE = 20f;
    public const float TIME_BETWEEN_WAVES = 40f;
    public const float FIRST_WAVE_WITCH_COUNT = 4;
    public const float FIRST_WAVE_KNIGHT_COUNT = 6;

    // this results in first siege appearing in 5th wave
    public const float FIRST_WAVE_SIEGE_COUNT = 1 / ( WAVE_GROWTH_FACTOR * WAVE_GROWTH_FACTOR * WAVE_GROWTH_FACTOR * WAVE_GROWTH_FACTOR) + 0.05f;
    public const float WAVE_GROWTH_FACTOR = 1.125f;

    public const int WAVE_SPAWN_RADIUS = 5;


    public const int ATTACK_MOVE_COUNT_STATE_CHANGE = 5;
    public const int COMMAND_PROXIMITY_RANGE = 5;
    public const int COMMAND_MAX_OUTPOST_BEFORE_ATTACK = 2;
    public const int COMMAND_FUTURE_TILE_LOCATION = 5;

    #endregion

    #region Buildings

    public const float BUILDING_DEFAULT_HEALTH = 3f;

    public const int CASTLE_SIZE_X = 3;
    public const int CASTLE_SIZE_Y = 3;
    public const float CASTLE_HEALTH = 50f;


    public const float OUTPOST_HEALTH = 20f;
    public const int OUTPOST_WOOD_COST = 5;
    public const int OUTPOST_STONE_COST = 5;
    public const int OUTPOST_FOOD_UPKEEP = 1;
    public const float OUTPOST_SHOOTING_RANGE = 3f;
    public const float OUTPOST_SHOOTING_RATE = .5f;
    public const float OUTPOST_ARROW_DAMAGE = 1f;
    public const float OUTPOST_ARROW_SPEED = 6f;
    public const float OUTPOST_BUILD_VALUE = 10f;

    public const int WALL_WOOD_COST = 0;
    public const int WALL_STONE_COST = 2;
    public const float WALL_HEALTH = 30f;
    public const float WALL_BUILD_VALUE = 4f;

    public const int FARM_WOOD_COST = 4;
    public const int FARM_STONE_COST = 2;
    public const int FARM_FOOD_UPKEEP = -4;
    public const float FARM_HEALTH = 10f;
    public const float FARM_BUILD_VALUE = 10f;

    public const int BRIDGE_WOOD_COST = 2;
    public const int BRIDGE_STONE_COST = 0;
    public const float BRIDGE_HEALTH = 5f;
    public const float BRIDGE_BUILD_VALUE = 10f;

    public const int HOSPITAL_WOOD_COST = 8;
    public const int HOSPITAL_STONE_COST = 2;
    public const float HOSPITAL_HEALTH = 10f;
    public const float HOSPITAL_BUILD_VALUE = 20f;
    public const float HOSPITAL_HEAL_RATE = 5f;
    public const float HOSPTIAL_HEAL_RANGE = 3f;

    public const int RESOURCE_BUILDING_WOOD_COST = 10;
    public const int RESOURCE_BUILDING_STONE_COST = 10;
    public const float RESOURCE_BUILDING_HEALTH = 10f;
    public const float RESOURCE_BUILDING_BUILD_VALUE = 20f;
    public const float RESOURCE_BUILDING_COLLECTION_TIME = 10f;
    public const float RESOURCE_BUILDING_COLLECTION_RANGE = 4f;

    public const int RESOURCE_WOOD_GATHER_BASE_RATE = 1;
    public const int RESOURCE_STONE_GATHER_BASE_RATE = 1;

    public const float RESOURCE_WOOD_GATHER_DURATION = .5f;
    public const float RESOURCE_STONE_GATHER_DURATION = .5f;

    public const float RESOURCE_WOOD_REGENERATION_TIME = 5f;
    public const float RESOURCE_STONE_REGENERATION_TIME = 5f;

    #endregion

    #region Entities

    // TODO: Enemy move types
    public const float PLAYER_WALK_SPEED = 4f;
    public const float PLAYER_ROTATE_SPEED = 3f * MathHelper.Pi;
    public const float PLAYER_HEALTH = 15f;
    public const float PLAYER_DAMAGE = 1f;
    public const float PLAYER_ATTACK_DURATION = 1f;

    public const float PLAYER_PLACE_BUILDING_COOLDOWN = .5f;
    public const float PLAYER_BUILD_COOLDOWN = .5f;
    public const float PLAYER_BUILD_STRENGTH = 1f;

    public const int RESURRECTION_COST_WOOD = 50;
    public const int RESURRECTION_COST_STONE = 50;
    public const int RESURRECTION_COST_FOOD = 10;
    public const float PLAYER_AFTER_RESURRECTION_HEALTH = .2f;

    public const float BUILDING_INTERACTION_COOLDOWN = .5f;

    public static readonly EnemyStats ENEMY_CATAPULT_STATS = new EnemyStats(
        walkSpeed: 1f,
        rotateSpeed: 1f * MathHelper.Pi,
        health: 20f,
        damageAgainstPlayer: 1f,
        damageAgainstBuilding: 2f,
        attackRange: 4f,
        attackDuration: 8f,
        projectileSpeed: 6f);
    public static readonly EnemyStats ENEMY_WITCH_STATS = new EnemyStats(
        walkSpeed: 2f,
        rotateSpeed: 3f * MathHelper.Pi,
        health: 5f,
        damageAgainstPlayer: 1f,
        damageAgainstBuilding: 1f,
        attackRange: 2f,
        attackDuration: 4f,
        projectileSpeed: 8f);
    public static readonly EnemyStats ENEMY_KNIGHTS_STATS = new EnemyStats(
        walkSpeed: 1.5f,
        rotateSpeed: 3f * MathHelper.Pi,
        health: 15f,
        damageAgainstPlayer: 1f,
        damageAgainstBuilding: 1f,
        attackRange: .25f,
        attackDuration: 2f,
        projectileSpeed: 0f);
    public static readonly EnemyStats TUTORIAL_GUY_STATS = new EnemyStats(
        walkSpeed: 1f,
        rotateSpeed: 3f * MathHelper.Pi,
        health: 5f,
        damageAgainstPlayer: 1f,
        damageAgainstBuilding: 1f,
        attackRange: 2f,
        attackDuration: 4f,
        projectileSpeed: 8f);

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
        public readonly float PROJECTILE_SPEED;

        public EnemyStats(float walkSpeed, float rotateSpeed, float health, float damageAgainstPlayer, float damageAgainstBuilding, float attackRange, float attackDuration, float projectileSpeed)
        {
            WALK_SPEED = walkSpeed;
            ROTATE_SPEED = rotateSpeed;
            HEALTH = health;
            DAMAGE_AGAINST_PLAYER = damageAgainstPlayer;
            DAMAGE_AGAINST_BUILDINGS = damageAgainstBuilding;
            ATTACK_RANGE = attackRange;
            ATTACK_DURATION = attackDuration;
            PROJECTILE_SPEED = projectileSpeed;
        }
    }
}
