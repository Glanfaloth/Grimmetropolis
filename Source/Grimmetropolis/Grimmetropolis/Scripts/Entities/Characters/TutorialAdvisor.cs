using Microsoft.Xna.Framework;

using System.Collections.Generic;

public enum TutorialAdvisorTask
{
    Waiting,
    Walking,
}

public class TutorialAdvisor : Enemy
{
    public override EnemyMove.Type Actions => EnemyMove.Type.Run;
    public override string MeshName => "TutorialAdvicer";

    private TutorialAdvisorTask _tutorialAdvicerTask = TutorialAdvisorTask.Waiting;
    private Vector2 _targetPosition = Vector2.Zero;

    private float _tickTimer = 1f;
    private float _tickDuration = 1f;

    public override void Initialize()
    {
        base.Initialize();

        _controller = null;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _tickTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_tutorialAdvicerTask == TutorialAdvisorTask.Walking) MoveToTargetPosition(gameTime);

        if (_tickTimer <= 0f)
        {
            if (_tutorialAdvicerTask == TutorialAdvisorTask.Waiting)
            {
                if (TDRandom.RandomFloat() < .3f) FindTargetMapTile();
            }

            _tickTimer += _tickDuration;
        }
    }

    public override void Destroy()
    {
        base.Destroy();

        GameManager.Instance.ResourcePool += new ResourcePile(0, 0, 10);
    }

    private void FindTargetMapTile()
    {
        Point mapTilePosition = GameManager.Instance.Map.GetMapTile(TDObject.Transform.Position.GetXY()).Position;
        List<Point> possibleTargetPositions = new List<Point>();

        possibleTargetPositions.Add(mapTilePosition + new Point(1, 0));
        possibleTargetPositions.Add(mapTilePosition + new Point(-1, 0));
        possibleTargetPositions.Add(mapTilePosition + new Point(0, 1));
        possibleTargetPositions.Add(mapTilePosition + new Point(0, -1));

        for (int i = possibleTargetPositions.Count - 1; i >= 0; i--)
        {
            if (possibleTargetPositions[i].X < 0 || possibleTargetPositions[i].X >= GameManager.Instance.Map.Width
                || possibleTargetPositions[i].Y < 0 || possibleTargetPositions[i].Y >= GameManager.Instance.Map.Height) possibleTargetPositions.RemoveAt(i);
            else if (GameManager.Instance.Map.MapTiles[possibleTargetPositions[i].X, possibleTargetPositions[i].Y].Structure != null) possibleTargetPositions.RemoveAt(i);
            else if (GameManager.Instance.Map.MapTiles[possibleTargetPositions[i].X, possibleTargetPositions[i].Y].Type == MapTileType.Water) possibleTargetPositions.RemoveAt(i);
        }

        Point targetMapTilePosition = possibleTargetPositions[TDRandom.RandomInt(possibleTargetPositions.Count)];
        _targetPosition = GameManager.Instance.Map.MapTiles[targetMapTilePosition.X, targetMapTilePosition.Y].TDObject.Transform.Position.GetXY();
        _tutorialAdvicerTask = TutorialAdvisorTask.Walking;
    }

    private void MoveToTargetPosition(GameTime gameTime)
    {
        if (Vector2.Distance(_targetPosition, TDObject.Transform.Position.GetXY()) < .05f)
        {
            Move(Vector2.Zero, gameTime);
            _tutorialAdvicerTask = TutorialAdvisorTask.Waiting;
        }
        else Move(Vector2.Normalize(_targetPosition - TDObject.Transform.Position.GetXY()), gameTime);
    }
}