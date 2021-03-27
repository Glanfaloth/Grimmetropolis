using Microsoft.Xna.Framework;


public class Map : TDComponent
{

    public int Width { get; private set; }
    public int Height { get; private set; }

    public MapTile[,] mapTiles { get; private set; }

    private int[,] _loadedMap;

    //public string mapPath = "Content/Maps/testmap.txt";

    public override void Initialize()
    {
        base.Initialize();

        /*_loadedMap = new int [,]
            { { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 2, 2, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
              { 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
              { 2, 2, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
              { 2, 2, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
              { 2, 2, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
              { 2, 2, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
              { 2, 2, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 } };*/

        // pathing test map
        _loadedMap = new int[,]
            { { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0 , 0 , 0 },
              { 0, 0, 0, 2, 2, 0, 1, 1, 0, 0 , 0 , 0 },
              { 0, 1, 0, 2, 2, 1, 1, 0, 0, 0 , 0 , 0 },
              { 0, 1, 0, 0, 0, 1, 1, 0, 0, 0 , 0 , 0 },
              { 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 , 0 , 0 },
              { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 , 0 , 0 },
              { 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 , 0 , 0 },
              { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 , 0 , 0 },
              { 2, 2, 0, 1, 1, 0, 0, 1, 1, 0 , 0 , 0 },
              { 2, 2, 0, 0, 1, 0, 0, 0, 0, 0 , 0 , 0 } };

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
        mapTiles = new MapTile[Width, Height];

        Vector3 corner = new Vector3(-.5f * Width, -.5f * Height, 0);
        Vector3 offcenter = new Vector3(.5f, .5f, 0f);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector3 position = TDObject.Transform.LocalPosition + corner + offcenter + new Vector3(x, y, 0f);
                TDObject mapTileObject = (MapTileType)_loadedMap[x, y] switch
                {
                    MapTileType.Ground => PrefabFactory.CreatePrefab(PrefabType.MapTileGround, position, Quaternion.Identity, TDObject.Transform),
                    MapTileType.Water => PrefabFactory.CreatePrefab(PrefabType.MapTileWater, position, Quaternion.Identity, TDObject.Transform),
                    MapTileType.Stone => PrefabFactory.CreatePrefab(PrefabType.MapTileStone, position, Quaternion.Identity, TDObject.Transform),
                    _ => PrefabFactory.CreatePrefab(PrefabType.MapTileGround, position, Quaternion.Identity, TDObject.Transform),
                };
                mapTiles[x, y] = mapTileObject.GetComponent<MapTile>();
            }
        }
    }

    public Point GetEnemyTarget()
    {
        // TODO: improve this to be artifact location
        return new Point((int)-.5f * Height, (int)-.5f * Width);
    }
}