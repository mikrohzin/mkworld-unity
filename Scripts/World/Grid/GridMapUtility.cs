using UnityEngine;

public static class GridMapUtility
{
    public static Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x);
        int y = Mathf.RoundToInt(worldPosition.z);
        return new Vector2Int(x, y);
    }

    public static Vector3 GridToWorld(int x, int y, int floor)
    {
        float floorHeightOffset = floor * 1.0f;
        return new Vector3(x, floorHeightOffset, y);
    }

    public static int ChebyshevDistance(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return Mathf.Max(dx, dy);
    }

    public static bool CanWalkTo(int floor, int x, int y)
    {
        if (MapManager.Instance == null || MapManager.Instance.CurrentMap == null)
            return false;

        if (TileDatabase.Instance == null)
            return false;

        MapData map = MapManager.Instance.CurrentMap;

        if (!map.IsValidPosition(floor, x, y))
            return false;

        MapTileData tileData = map.GetTile(floor, x, y);

        if (tileData == null)
            return false;

        if (tileData.groundTileId == 0)
            return false;

        TileDefinition groundTile = TileDatabase.Instance.GetTile(tileData.groundTileId);
        if (groundTile == null || !groundTile.walkable)
            return false;

        if (tileData.blockerTileId != 0)
        {
            TileDefinition blockerTile = TileDatabase.Instance.GetTile(tileData.blockerTileId);
            if (blockerTile != null && blockerTile.occupiesTile)
                return false;
        }

        if (tileData.objectTileId != 0)
        {
            TileDefinition objectTile = TileDatabase.Instance.GetTile(tileData.objectTileId);
            if (objectTile != null && objectTile.occupiesTile)
                return false;
        }

        return true;
    }
}