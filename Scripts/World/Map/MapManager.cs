using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public int mapWidth = 128;
    public int mapHeight = 128;
    public int floorCount = 8;
    public int chunkSize = 16;

    public MapData CurrentMap { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        CreateEmptyMap();
    }

    public void CreateEmptyMap()
    {
        CurrentMap = new MapData(mapWidth, mapHeight, floorCount, chunkSize);

        Debug.Log($"Map created: {mapWidth}x{mapHeight}, floors={floorCount}, chunkSize={chunkSize}");
    }

    public void FillFloorWithGround(int floor, int groundTileId)
    {
        if (CurrentMap == null)
            return;

        for (int y = 0; y < CurrentMap.height; y++)
        {
            for (int x = 0; x < CurrentMap.width; x++)
            {
                CurrentMap.SetGround(floor, x, y, groundTileId);
            }
        }

        Debug.Log($"Filled floor {floor} with ground tile id {groundTileId}");
    }
}