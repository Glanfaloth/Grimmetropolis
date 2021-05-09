using Microsoft.Xna.Framework;

public enum WallOrientation
{
    Horizontal,
    Vertical,
    Center
}

public class Wall : Building
{
    public override float BuildTime => Config.WALL_BUILD_VALUE;
    public override ResourcePile GetResourceCost() => new ResourcePile(Config.WALL_WOOD_COST, Config.WALL_STONE_COST);


    private WallOrientation _orientation = WallOrientation.Center;

    public override void Initialize()
    {
        BaseHealth = Config.WALL_HEALTH;
        Health = Config.WALL_HEALTH;

        ReorientWall();

        base.Initialize();

        ReorientNeighbouringWalls();
    }

    public override void Destroy()
    {
        base.Destroy();

        ReorientNeighbouringWalls();
    }

    public void ReorientWall()
    {
        bool leftWall = Position.X >= 1
            && GameManager.Instance.Map.MapTiles[Position.X - 1, Position.Y].Structure is Wall;
        bool rightWall = Position.X <= GameManager.Instance.Map.Width - 2
            && GameManager.Instance.Map.MapTiles[Position.X + 1, Position.Y].Structure is Wall;
        bool upWall = Position.Y >= 1
            && GameManager.Instance.Map.MapTiles[Position.X, Position.Y - 1].Structure is Wall;
        bool downWall = Position.Y <= GameManager.Instance.Map.Width - 2
            && GameManager.Instance.Map.MapTiles[Position.X, Position.Y + 1].Structure is Wall;

        if ((leftWall || rightWall) && (upWall || downWall))
            _orientation = WallOrientation.Center;
        else if (leftWall && rightWall)
            _orientation = WallOrientation.Vertical;
        else if (upWall && downWall)
            _orientation = WallOrientation.Horizontal;
        else if (leftWall || rightWall)
            _orientation = WallOrientation.Vertical;
        else if (upWall || downWall)
            _orientation = WallOrientation.Horizontal;


        switch (_orientation)
        {
            case WallOrientation.Horizontal:
                Mesh.Model = TDContentManager.LoadModel("BuildingWall");
                TDObject.Transform.LocalRotation = Quaternion.Identity;
                break;
            case WallOrientation.Vertical:
                Mesh.Model = TDContentManager.LoadModel("BuildingWall");
                TDObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, MathHelper.PiOver2);
                break;
            case WallOrientation.Center:
                Mesh.Model = TDContentManager.LoadModel("BuildingCenterWall");
                TDObject.Transform.LocalRotation = Quaternion.Identity;
                break;
        }
    }

    public void ReorientNeighbouringWalls()
    {
        if (Position.X >= 1
            && GameManager.Instance.Map.MapTiles[Position.X - 1, Position.Y].Structure is Wall wallLeft)
            wallLeft.ReorientWall();
        if (Position.X <= GameManager.Instance.Map.Width - 2
            && GameManager.Instance.Map.MapTiles[Position.X + 1, Position.Y].Structure is Wall wallRight)
            wallRight.ReorientWall();
        if (Position.Y >= 1
            && GameManager.Instance.Map.MapTiles[Position.X, Position.Y - 1].Structure is Wall wallUp)
            wallUp.ReorientWall();
        if (Position.Y <= GameManager.Instance.Map.Width - 2
            && GameManager.Instance.Map.MapTiles[Position.X, Position.Y + 1].Structure is Wall wallDown)
            wallDown.ReorientWall();
    }
}