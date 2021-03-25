using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;


public class Map : TDComponent
{

    public int width;
    public int height;
    public float cellSize = 1f;
    public TDObject[,] mapTiles;
    public int[,] mapArray = {{0,0,0,0,0,0,0,2,2,0,},
{0,0,0,1,1,0,2,2,0,0,},
{0,0,0,1,1,2,2,0,0,0,},
{0,0,0,0,0,2,2,0,0,0,},
{0,0,0,0,0,2,2,0,0,0,},
{0,0,0,0,0,0,2,2,0,0,},
{0,0,0,0,0,0,2,2,0,0,},
{0,0,0,0,0,0,2,2,0,0,},
{1,1,0,0,0,0,0,2,2,0,},
{1,1,0,0,0,0,0,2,2,0,},};

    //public string mapPath = "Content/Maps/testmap.txt";

    public Map(TDObject tdObject) : base(tdObject)
    {
        //using (Stream fileStream = TitleContainer.OpenStream("Content/Maps/testmap.txt"))
        //    LoadTiles(fileStream);
        LoadTiles(mapArray);
    }

    // load map tiles
    public void LoadTiles(int[,] mapArray)
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
        height = mapArray.GetLength(0);
        width = mapArray.GetLength(1);
        mapTiles = new TDObject[width, height];
        Vector3 center = new Vector3(width * cellSize / 2, height * cellSize / 2, 0f);

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++)
            {
                //char type = lines[y][x];
                int type = mapArray[y, x];
                TDObject tile = PrefabFactory.CreatePrefab(PrefabType.Empty);
                if (type == 1)
                    tile.Components.Add(new TDMesh(tile, "Stone", "BaseTexture"));
                else if (type == 2)
                    tile.Components.Add(new TDMesh(tile, "Water", "BaseTexture"));
                else
                    tile.Components.Add(new TDMesh(tile, "Ground", "BaseTexture"));

                tile.Transform.Position = center - new Vector3(x * cellSize, y * cellSize, 0f);
                mapTiles[x,y] = tile;
            }
        }

    }

}