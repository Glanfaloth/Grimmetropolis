using Microsoft.Xna.Framework;

public enum BridgeOrientation
{
    Horizontal,
    Vertical
}

public class Bridge : Building
{
    public override MapTileType GetRequiredMapTileType() => MapTileType.Water;
    public override bool CanBeAttacked => false;

    private TDTransform[] _bridgeAttachementTransforms = new TDTransform[4];

    private TDMesh[] _bridgeAttachementMeshes = new TDMesh[4];

    public BridgeOrientation Orientation = BridgeOrientation.Vertical;
    public override Point Position
    {
        get => base.Position;
        set
        {
            base.Position = value;
            if (_bridgeAttachementMeshes[0] != null) AdjustBridgeAttachements();
        }
    }

    public override void Initialize()
    {
        IsPassable = true;

        for (int i = 0; i < _bridgeAttachementTransforms.Length; i++)
        {
            CreateBuildingPart(Mesh.Model, Mesh.Texture, "BridgeAttachement", out _bridgeAttachementTransforms[i], out _bridgeAttachementMeshes[i]);
        }
        _bridgeAttachementTransforms[0].LocalPosition = .5f * Vector3.Left;
        _bridgeAttachementTransforms[1].LocalPosition = .5f * Vector3.Down;
        _bridgeAttachementTransforms[2].LocalPosition = .5f * Vector3.Right;
        _bridgeAttachementTransforms[3].LocalPosition = .5f * Vector3.Up;
        _bridgeAttachementTransforms[1].LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Forward, 3f * MathHelper.PiOver2);
        _bridgeAttachementTransforms[2].LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Forward, MathHelper.Pi);
        _bridgeAttachementTransforms[3].LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Forward,  MathHelper.PiOver2);

        CreateMainBuildingPart(Mesh.Model, Mesh.Texture, "Bridge");

        AdjustBridgeAttachements();

        base.Initialize();
    }

    protected override void SetAsPreview()
    {
        base.SetAsPreview();
        foreach (TDMesh bridgeAttachementMesh in _bridgeAttachementMeshes)
        {
            bridgeAttachementMesh.IsPreview = true;
            bridgeAttachementMesh.BaseColor = Mesh.BaseColor;
        }
    }

    protected override void SetAsBlueprint()
    {
        base.SetAsBlueprint();
        foreach (TDMesh bridgeAttachementMesh in _bridgeAttachementMeshes)
        {
            bridgeAttachementMesh.IsPreview = true;
            bridgeAttachementMesh.BaseColor = Mesh.BaseColor;
        }
    }

    public override bool TryBuild(float buildStrength)
    {
        if (_buildProgress + buildStrength >= BuildTime)
        {
            foreach (TDMesh bridgeAttachementMesh in _bridgeAttachementMeshes)
            {
                bridgeAttachementMesh.IsPreview = false;
                bridgeAttachementMesh.BaseColor = Vector4.One;
            }

            IsPreview = false;
            GameManager.Instance.Map.MapTiles[Position.X, Position.Y].AdjustCollider();
            GameManager.Instance.Map.MapTiles[Position.X, Position.Y].UpdateGraph();
        }

        return base.TryBuild(buildStrength);
    }

    public override void Highlight(bool highlight)
    {
        base.Highlight(highlight);
        foreach (TDMesh bridgeAttachementMesh in _bridgeAttachementMeshes)
        {
            bridgeAttachementMesh.Highlight(highlight);
        }
    }
    private void AdjustBridgeAttachements()
    {
        MapTile mapTile = GameManager.Instance.Map.MapTiles[Position.X, Position.Y];
        MapTile mapTileUp = mapTile.GetNeighbouringMapTile(new Point(-1, 0));
        MapTile mapTileLeft = mapTile.GetNeighbouringMapTile(new Point(0, -1));
        MapTile mapTileDown = mapTile.GetNeighbouringMapTile(new Point(1, 0));
        MapTile mapTileRight = mapTile.GetNeighbouringMapTile(new Point(0, 1));
        _bridgeAttachementMeshes[0].IsShown = mapTileUp?.Type == MapTileType.Ground;
        _bridgeAttachementMeshes[1].IsShown = mapTileLeft?.Type == MapTileType.Ground;
        _bridgeAttachementMeshes[2].IsShown = mapTileDown?.Type == MapTileType.Ground;
        _bridgeAttachementMeshes[3].IsShown = mapTileRight?.Type == MapTileType.Ground;

        if (!IsPreview)
        {
            if (mapTileUp?.Structure is Bridge bridge0) Orientation = bridge0.Orientation;
            else if (mapTileDown?.Structure is Bridge bridge1) Orientation = bridge1.Orientation;
            else if (mapTileLeft?.Structure is Bridge bridge2) Orientation = bridge2.Orientation;
            else if (mapTileRight?.Structure is Bridge bridge3) Orientation = bridge3.Orientation;
            else if (_bridgeAttachementMeshes[0].IsShown && _bridgeAttachementMeshes[2].IsShown) Orientation = BridgeOrientation.Vertical;
            else if (_bridgeAttachementMeshes[1].IsShown && _bridgeAttachementMeshes[3].IsShown) Orientation = BridgeOrientation.Horizontal;
            else if (_bridgeAttachementMeshes[0].IsShown || _bridgeAttachementMeshes[2].IsShown) Orientation = BridgeOrientation.Vertical;
            else if (_bridgeAttachementMeshes[1].IsShown || _bridgeAttachementMeshes[3].IsShown) Orientation = BridgeOrientation.Horizontal;

            if (Orientation == BridgeOrientation.Horizontal)
            {
                foreach (TDTransform bridgeAttachementTransform in _bridgeAttachementTransforms) bridgeAttachementTransform.Parent = null;
                TDObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Forward, MathHelper.PiOver2);
                foreach (TDTransform bridgeAttachementTransform in _bridgeAttachementTransforms) bridgeAttachementTransform.Parent = TDObject.Transform;
            }
        }
    }
}
