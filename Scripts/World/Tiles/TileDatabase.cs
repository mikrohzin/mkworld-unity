using System.Collections.Generic;
using UnityEngine;

public class TileDatabase : MonoBehaviour
{
    public static TileDatabase Instance;

    [Header("Tile Definitions")]
    public TileDefinition[] tiles;

    private Dictionary<int, TileDefinition> tileById;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        BuildDatabase();
    }

    void BuildDatabase()
    {
        tileById = new Dictionary<int, TileDefinition>();

        foreach (var tile in tiles)
        {
            if (tile == null)
                continue;

            if (tileById.ContainsKey(tile.id))
            {
                Debug.LogError("Duplicate Tile ID: " + tile.id);
                continue;
            }

            tileById.Add(tile.id, tile);
        }
    }

    public TileDefinition GetTile(int id)
    {
        if (tileById.TryGetValue(id, out TileDefinition tile))
            return tile;

        Debug.LogError("Tile not found: " + id);
        return null;
    }
}