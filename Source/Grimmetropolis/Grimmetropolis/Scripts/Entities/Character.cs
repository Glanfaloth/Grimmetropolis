using Microsoft.Xna.Framework;

using System;

public abstract class Character : TDComponent
{
    public float _health = 3f;
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health < 0f) TDObject.Destroy();
        }
    }

    private float _lookingAngle = 0f;

    private float _walkSpeed = 4f;
    private float _rotateSpeed = 3f * MathHelper.Pi;

    public TDCylinderCollider InteractionCollider;

    public override void Initialize()
    {
        base.Initialize();

        TDObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, _lookingAngle);
    }

    protected void Move(Vector2 direction, GameTime gameTime)
    {
        if (direction.LengthSquared() > float.Epsilon)
        {
            float targetAngle = MathF.Atan2(direction.Y, direction.X);
            if (targetAngle - _lookingAngle > MathHelper.Pi) _lookingAngle += MathHelper.TwoPi;
            else if (_lookingAngle - targetAngle > MathHelper.Pi) _lookingAngle -= MathHelper.TwoPi;

            if (targetAngle > _lookingAngle) _lookingAngle = MathHelper.Min(_lookingAngle + _rotateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, targetAngle);
            else if (targetAngle < _lookingAngle) _lookingAngle = MathHelper.Max(_lookingAngle - _rotateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, targetAngle);

            TDObject.Transform.LocalPosition += _walkSpeed * new Vector3(direction, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            TDObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, _lookingAngle);
        }
    }

    protected void Attack()
    {

    }

    protected void Build()
    {
        MapTile mapTile = GameManager.Instance.Map.GetMapTile(InteractionCollider.CenterXY);
        if (mapTile.Building == null)
        {
            TDObject buildingObject = PrefabFactory.CreatePrefab(PrefabType.Outpost, GameManager.Instance.BuildingTransform);
            Building building = buildingObject.GetComponent<Building>();
            building.Position = mapTile.Position;
            building.PlaceBuilding();
        }
    }
}
