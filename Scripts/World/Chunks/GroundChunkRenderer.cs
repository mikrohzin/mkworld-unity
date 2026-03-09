using System.Collections.Generic;
using UnityEngine;

public class GroundChunkRenderer : MonoBehaviour
{
    public int floor;
    public int chunkX;
    public int chunkY;
    public int chunkSize;

    private readonly List<GameObject> spawnedTiles = new List<GameObject>();

    public void Initialize(int floor, int chunkX, int chunkY, int chunkSize)
    {
        this.floor = floor;
        this.chunkX = chunkX;
        this.chunkY = chunkY;
        this.chunkSize = chunkSize;
    }

    public void RenderChunk()
    {
        ClearChunk();

        if (MapManager.Instance == null || MapManager.Instance.CurrentMap == null)
        {
            Debug.LogError("MapManager or CurrentMap is null.");
            return;
        }

        if (TileDatabase.Instance == null)
        {
            Debug.LogError("TileDatabase.Instance is null.");
            return;
        }

        MapData map = MapManager.Instance.CurrentMap;

        int startX = chunkX * chunkSize;
        int startY = chunkY * chunkSize;
        int endX = Mathf.Min(startX + chunkSize, map.width);
        int endY = Mathf.Min(startY + chunkSize, map.height);

        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                MapTileData tileData = map.GetTile(floor, x, y);

                if (tileData == null || tileData.groundTileId == 0)
                    continue;

                TileDefinition tileDef = TileDatabase.Instance.GetTile(tileData.groundTileId);

                if (tileDef == null || tileDef.groundPrefab == null)
                    continue;

                Vector3 worldPosition = GetWorldPosition(x, y, floor, tileDef.height);

                GameObject tileObject = Instantiate(
                    tileDef.groundPrefab,
                    worldPosition,
                    Quaternion.identity,
                    transform
                );

                tileObject.name = $"Ground_{tileDef.tileName}_{floor}_{x}_{y}";
                spawnedTiles.Add(tileObject);
            }
        }
    }

    public void ClearChunk()
    {
        for (int i = spawnedTiles.Count - 1; i >= 0; i--)
        {
            if (spawnedTiles[i] != null)
                DestroyImmediate(spawnedTiles[i]);
        }

        spawnedTiles.Clear();
    }

    private Vector3 GetWorldPosition(int x, int y, int floor, float tileHeight)
    {
        float floorHeightOffset = floor * 1.0f;
        return new Vector3(x, floorHeightOffset + tileHeight, y);
    }
}