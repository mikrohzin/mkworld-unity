using System.Collections.Generic;
using UnityEngine;

public class EntityOccupancyManager : MonoBehaviour
{
    public static EntityOccupancyManager Instance { get; private set; }

    private readonly Dictionary<string, GameObject> occupiedTiles = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private string GetKey(int floor, int x, int y)
    {
        return $"{floor}_{x}_{y}";
    }

    public bool IsOccupied(int floor, int x, int y)
    {
        return occupiedTiles.ContainsKey(GetKey(floor, x, y));
    }

    public GameObject GetOccupant(int floor, int x, int y)
    {
        occupiedTiles.TryGetValue(GetKey(floor, x, y), out GameObject occupant);
        return occupant;
    }

    public bool TryRegister(int floor, int x, int y, GameObject entity)
    {
        string key = GetKey(floor, x, y);

        if (occupiedTiles.TryGetValue(key, out GameObject current))
        {
            if (current == entity)
                return true;

            return false;
        }

        occupiedTiles[key] = entity;
        return true;
    }

    public void Unregister(int floor, int x, int y, GameObject entity)
    {
        string key = GetKey(floor, x, y);

        if (!occupiedTiles.TryGetValue(key, out GameObject current))
            return;

        if (current == entity)
        {
            occupiedTiles.Remove(key);
        }
    }

    public bool Move(int floor, Vector2Int from, Vector2Int to, GameObject entity)
    {
        string fromKey = GetKey(floor, from.x, from.y);
        string toKey = GetKey(floor, to.x, to.y);

        if (occupiedTiles.TryGetValue(toKey, out GameObject targetEntity))
        {
            if (targetEntity != entity)
                return false;
        }

        if (occupiedTiles.TryGetValue(fromKey, out GameObject fromEntity))
        {
            if (fromEntity == entity)
            {
                occupiedTiles.Remove(fromKey);
            }
        }

        occupiedTiles[toKey] = entity;
        return true;
    }

    public void ForceClearAll()
    {
        occupiedTiles.Clear();
    }
}