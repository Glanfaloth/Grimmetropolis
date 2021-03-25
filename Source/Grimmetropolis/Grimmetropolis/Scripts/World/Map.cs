using Microsoft.Xna.Framework;


public class Map : TDComponent
{

    public int Width { get; private set; }
    public int Height { get; private set; }

    public MapTile[,] mapTiles;

    private int[,] loadedMap;

    //public string mapPath = "Content/Maps/testmap.txt";

    public Map(TDObject tdObject) : base(tdObject)
    {
        loadedMap = new int [,]
            { { 0, 0, 0, 0, 0, 0, 0, 2, 2, 0 , 0 , 0 },
              { 0, 0, 0, 1, 1, 0, 2, 2, 0, 0 , 0 , 0 },
              { 0, 0, 0, 1, 1, 2, 2, 0, 0, 0 , 0 , 0 },
              { 0, 0, 0, 0, 0, 2, 2, 0, 0, 0 , 0 , 0 },
              { 0, 0, 0, 0, 0, 2, 2, 0, 0, 0 , 0 , 0 },
              { 0, 0, 0, 0, 0, 0, 2, 2, 0, 0 , 0 , 0 },
              { 0, 0, 0, 0, 0, 0, 2, 2, 0, 0 , 0 , 0 },
              { 0, 0, 0, 0, 0, 0, 2, 2, 0, 0 , 0 , 0 },
              { 1, 1, 0, 0, 0, 0, 0, 2, 2, 0 , 0 , 0 },
              { 1, 1, 0, 0, 0, 0, 0, 2, 2, 0 , 0 , 0 } };

        //using (Stream fileStream = TitleContainer.OpenStream("Content/Maps/testmap.txt"))
        //    LoadTiles(fileStream);
        LoadMap();
    }

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
        Width = loadedMap.GetLength(0);
        Height = loadedMap.GetLength(1);
        mapTiles = new MapTile[Width, Height];

        Vector3 center = new Vector3(-.5f * Height, -.5f * Width, 0);
        Vector3 offcenter = .5f * Vector3.One;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector3 position = center + offcenter + new Vector3(x, y, 0f);
                TDObject tile = PrefabFactory.CreatePrefab(PrefabType.Empty, position, Quaternion.Identity, TDObject.Transform);
                switch (loadedMap[x, y])
                {
                    case 1: tile.Components.Add(new TDMesh(tile, "MapTileStone", "ColorPaletteTexture")); break;
                    case 2: tile.Components.Add(new TDMesh(tile, "MapTileWater", "ColorPaletteTexture")); break;
                    default: tile.Components.Add(new TDMesh(tile, "MapTileGround", "ColorPaletteTexture")); break;
                }
                MapTile mapTile = new MapTile(tile, x, y);
                tile.Components.Add(mapTile);
                mapTiles[x, y] = mapTile;

            }
        }
    }
}