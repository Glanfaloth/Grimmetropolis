using Microsoft.Xna.Framework;

using System;
using System.Diagnostics;

public enum ProjectileState
{
    Moving,
    Stuck
}

public class Projectile : TDComponent
{
    public Vector3 StartPosition = Vector3.Zero;
    public Character TargetCharacter = null;

    public TDCylinderCollider Collider;

    public float Speed = Config.OUTPOST_ARROW_SPEED;
    public float Damage = Config.OUTPOST_ARROW_DAMAGE;

    private Vector3 _direction = Vector3.Zero;
    private float _distance = 0f;
    private float _azimuthAngle = 0f;
    private float _elevationAngle = 0f;

    private ProjectileState _state = ProjectileState.Moving;
    private float _lifeTime = 10f;


    public override void Initialize()
    {
        base.Initialize();

        if (TargetCharacter?.TDObject == null)
        {
            TDObject.Destroy();
            return;
        }

        Collider.IsTrigger = true;
        Collider.Radius = .05f;
        Collider.Height = .1f;
        Collider.collisionEvent += HitCollider;

        TDObject.Transform.Position = StartPosition;

        CalculateValues();
        _elevationAngle = -MathF.Atan2(_direction.Z, _direction.GetXY().Length());

        TDObject.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(0f, _elevationAngle, _azimuthAngle);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_state == ProjectileState.Moving) UpdatePosition(gameTime);
        else if (_state == ProjectileState.Stuck)
        {
            _lifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_lifeTime <= 0f)
            {
                TDObject.Destroy();
            }
        }
    }


    private void UpdatePosition(GameTime gameTime)
    {
        CalculateValues();

        TDObject.Transform.Position += Speed * _direction / _distance * (float)gameTime.ElapsedGameTime.TotalSeconds;
        TDObject.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(0f, _elevationAngle, _azimuthAngle);
    }

    private void CalculateValues()
    {
        if (TargetCharacter.TDObject != null)
        {
            _direction = TargetCharacter.TDObject.Transform.Position + TargetCharacter.OffsetTarget - TDObject.Transform.Position;
            _distance = _direction.Length();
            _azimuthAngle = MathF.Atan2(_direction.Y, _direction.X);
        }
    }

    private void HitCollider(TDCollider collider1, TDCollider collider2, float intersection)
    {
        if (_state == ProjectileState.Moving)
        {
            TDCollider oppositeCollider = Collider == collider2 ? collider1 : collider2;

            Enemy enemy = oppositeCollider.TDObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                _state = ProjectileState.Stuck;
                TDObject.Transform.Parent = enemy.TDObject.Transform;
                enemy.Health -= Config.OUTPOST_ARROW_DAMAGE;
            }
            else
            {
                MapTile mapTile = oppositeCollider.TDObject.GetComponent<MapTile>();
                // TODO: check when exactly it should collide with other things
                if (mapTile != null && mapTile.Type == MapTileType.Ground && mapTile.Structure == null)
                {
                    TDObject.Transform.Parent = mapTile.TDObject.Transform;
                    _state = ProjectileState.Stuck;
                }
            }
        }
    }
}
