using System;
using UnityEngine;

[Serializable]
public class MapData
{
    public int width;
    public int height;
    public int floors;
    public int chunkSize;

    [SerializeField]
    private MapTileData[] tiles;

    public MapData(int width, int height, int floors, int chunkSize)
    {
        this.width = width;
        this.height = height;
        this.floors = floors;
        this.chunkSize = chunkSize;

        tiles = new MapTileData[width * height * floors];

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new MapTileData();
        }
    }

    public bool IsValidPosition(int floor, int x, int y)
    {
        return floor >= 0 && floor < floors &&
               x >= 0 && x < width &&
               y >= 0 && y < height;
    }

    public MapTileData GetTile(int floor, int x, int y)
    {
        if (!IsValidPosition(floor, x, y))
        {
            Debug.LogError($"Invalid map position: floor={floor}, x={x}, y={y}");
            return null;
        }

        return tiles[GetIndex(floor, x, y)];
    }

    public void SetTile(int floor, int x, int y, MapTileData tileData)
    {
        if (!IsValidPosition(floor, x, y))
        {
            Debug.LogError($"Invalid map position: floor={floor}, x={x}, y={y}");
            return;
        }

        tiles[GetIndex(floor, x, y)] = tileData;
    }

    public void SetGround(int floor, int x, int y, int groundTileId)
    {
        if (!IsValidPosition(floor, x, y))
            return;

        tiles[GetIndex(floor, x, y)].groundTileId = groundTileId;
    }

    public void SetObject(int floor, int x, int y, int objectTileId)
    {
        if (!IsValidPosition(floor, x, y))
            return;

        tiles[GetIndex(floor, x, y)].objectTileId = objectTileId;
    }

    public void SetBlocker(int floor, int x, int y, int blockerTileId)
    {
        if (!IsValidPosition(floor, x, y))
            return;

        tiles[GetIndex(floor, x, y)].blockerTileId = blockerTileId;
    }

    public int GetIndex(int floor, int x, int y)
    {
        return floor * (width * height) + y * width + x;
    }
}