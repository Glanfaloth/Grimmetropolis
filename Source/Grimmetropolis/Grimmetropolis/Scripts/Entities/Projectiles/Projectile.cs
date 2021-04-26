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
    public ITarget TargetCharacter = null;

    public TDCylinderCollider Collider;

    public float Speed = 0;
    public float Damage = 0;

    private Vector3 _direction = Vector3.Zero;
    private float _distance = 0f;
    private float _azimuthAngle = 0f;
    private float _elevationAngle = 0f;

    private ProjectileState _state = ProjectileState.Moving;
    private float _lifeTime = 10f;

    // TODO: replace this temporary workaround.
    public bool IsEvilArrow { get; internal set; }

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
        Vector3 directionXY = new Vector3(_direction.GetXY(), 0f);
        _elevationAngle = MathF.Acos(Vector3.Dot(Vector3.Normalize(_direction), Vector3.Normalize(directionXY)));

        UpdateRotation();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_state == ProjectileState.Moving) UpdateTransform(gameTime);
        else if (_state == ProjectileState.Stuck)
        {
            _lifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_lifeTime <= 0f)
            {
                TDObject.Destroy();
            }
        }
    }

    private void UpdateTransform(GameTime gameTime)
    {
        CalculateValues();

        TDObject.Transform.Position += Speed * _direction / _distance * (float)gameTime.ElapsedGameTime.TotalSeconds;
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        TDObject.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, _azimuthAngle) * Quaternion.CreateFromAxisAngle(Vector3.Up, _elevationAngle);
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

            // TODO: enemies can shoot themselfes and other enemies
            Enemy enemy = oppositeCollider.TDObject.GetComponent<Enemy>();
            Player player = oppositeCollider.TDObject.GetComponent<Player>();
            if (enemy != null && !IsEvilArrow)
            {
                _state = ProjectileState.Stuck;
                TDObject.Transform.Parent = enemy.TDObject.Transform;
                enemy.Health -= Damage;
            }
            else if (player != null && IsEvilArrow)
            {
                _state = ProjectileState.Stuck;
                TDObject.Transform.Parent = player.TDObject.Transform;
                player.Health -= Damage;
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
                else if (mapTile != null && mapTile.Structure == TargetCharacter && mapTile.Structure is Building building)
                {
                    _state = ProjectileState.Stuck;
                    TDObject.Transform.Parent = building.TDObject.Transform;
                    building.Health -= Damage;
                }
            }
        }
    }
}
