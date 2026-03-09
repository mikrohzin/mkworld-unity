using UnityEngine;

[System.Serializable]
public class WorldTile
{
    public Vector2Int gridPosition;
    public bool walkable = true;
    public bool isProtectionZone = false;

    public GameObject tileVisual;
    public GameObject occupant;

    public WorldTile(Vector2Int gridPosition, bool walkable = true)
    {
        this.gridPosition = gridPosition;
        this.walkable = walkable;
    }

    public bool HasOccupant()
    {
        return occupant != null;
    }

    public bool IsBlocked()
    {
        if (!walkable)
            return true;

        if (occupant != null)
            return true;

        return false;
    }

    public void SetOccupant(GameObject obj)
    {
        occupant = obj;
    }

    public void ClearOccupant()
    {
        occupant = null;
    }
}