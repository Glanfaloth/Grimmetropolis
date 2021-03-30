using Microsoft.Xna.Framework;
using System;

public class Map : TDComponent
{

    public int Width { get; private set; }
    public int Height { get; private set; }

    public MapTile[,] MapTiles { get; private set; }

    private int[,] _loadedMap;

    public Vector3 Corner { get; private set; }
    public Vector3 Offcenter { get; private set; }

    //public string mapPath = "Content/Maps/testmap.txt";

    public override void Initialize()
    {
        base.Initialize();

        /*_loadedMap = new int [,]
            { { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 } };*/

        // pathing test map
        _loadedMap = new int[,]
            { { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0 , 0 , 0 },
              { 0, 0, 0, 0, 0, 0, 1, 1, 0, 0 , 0 , 0 },
              { 0, 1, 0, 0, 0, 1, 1, 0, 0, 0 , 0 , 0 },
              { 0, 1, 0, 0, 1, 1, 1, 0, 0, 0 , 0 , 0 },
              { 0, 1, 1, 1, 1, 0, 0, 1, 0, 0 , 0 , 0 },
              { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 , 0 , 0 },
              { 0, 1, 1, 1, 0, 0, 0, 1, 0, 0 , 0 , 0 },
              { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0 , 0 , 0 },
              { 0, 0, 0, 1, 1, 0, 0, 1, 1, 0 , 0 , 0 },
              { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 , 0 , 0 } };

        //using (Stream fileStream = TitleContainer.OpenStream("Content/Maps/testmap.txt"))
        //    LoadTiles(fileStream);

        LoadMap();
    }

    public override void Update(GameTime gameTime) { }

    // load map tiles
    public void LoadMap()
    {
        //List<string> lines = new List<string>();
        //using (StreamReader reader = new StreamReader(fileStream))
        //{
        //    string line = reader.ReadLine();
        //    width = line.Length;
        //    while (line != null)
        //    {
        //        lines.Add(line);
        //        if (line.Length != width)
        //            throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
        //        line = reader.ReadLine();
        //    }
        //}
        //height = lines.Count;
        Width = _loadedMap.GetLength(0);
        Height = _loadedMap.GetLength(1);
        MapTiles = new MapTile[Width, Height];

        Corner = new Vector3(-.5f * Width, -.5f * Height, 0);
        Offcenter = new Vector3(.5f, .5f, 0f);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector3 position = TDObject.Transform.LocalPosition + Corner + Offcenter + new Vector3(x, y, 0f);
                TDObject mapTileObject = (MapTileType)_loadedMap[x, y] switch
                {
                    MapTileType.Ground => PrefabFactory.CreatePrefab(PrefabType.MapTileGround, position, Quaternion.Identity, TDObject.Transform),
                    MapTileType.Water => PrefabFactory.CreatePrefab(PrefabType.MapTileWater, position, Quaternion.Identity, TDObject.Transform),
                    _ => PrefabFactory.CreatePrefab(PrefabType.MapTileGround, position, Quaternion.Identity, TDObject.Transform)
                };
                MapTile mapTile = mapTileObject.GetComponent<MapTile>();
                mapTile.Position = new Point(x, y);
                MapTiles[x, y] = mapTile;
            }
        }
    }

    public MapTile GetMapTile(Vector2 position)
    {
        int x = Math.Clamp((int)(position.X - TDObject.Transform.Position.X - Corner.X), 0, Width - 1);
        int y = Math.Clamp((int)(position.Y - TDObject.Transform.Position.Y - Corner.Y), 0, Height - 1);
        return MapTiles[x, y];
    }
    
    public Point GetEnemyTargetIndex()
    {
        // TODO: improve this to be artifact location
        return new Point(5, 6);
    }
    public bool IsInBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }

    //internal bool TryGetTileIndex(Vector3 worldLocation, out Point tileIndex)
    //{
    //    // TODO: improve this conversion
    //    var location = worldLocation - Corner;

    //    tileIndex = new Point((int)location.X, (int)location.Y);

    //    return (tileIndex.X >= 0 && tileIndex.Y >= 0 && tileIndex.X < Width && tileIndex.Y < Height);
    //}
}