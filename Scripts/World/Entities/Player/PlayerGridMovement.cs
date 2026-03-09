using System.Collections;
using UnityEngine;

public class PlayerGridMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveDuration = 0.15f;
    public int currentFloor = 0;
    public bool isMoving;

    [Header("Optional")]
    public bool snapToGridOnStart = true;

    private Vector2Int gridPosition;

    public Vector2Int GridPosition => gridPosition;

    void Start()
    {
        if (snapToGridOnStart)
        {
            SnapToGrid();
        }
        else
        {
            gridPosition = WorldToGrid(transform.position);
        }

        RegisterInitialOccupancy();
    }

    void Update()
    {
        if (isMoving)
            return;

        Vector2Int input = ReadInput();

        if (input == Vector2Int.zero)
            return;

        TryMove(input);
    }

    private Vector2Int ReadInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            return Vector2Int.up;

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            return Vector2Int.down;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            return Vector2Int.left;

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            return Vector2Int.right;

        return Vector2Int.zero;
    }

    public void SnapToGrid()
    {
        gridPosition = WorldToGrid(transform.position);
        transform.position = GridToWorld(gridPosition.x, gridPosition.y, currentFloor);
    }

    private void RegisterInitialOccupancy()
    {
        if (EntityOccupancyManager.Instance == null)
        {
            Debug.LogError("EntityOccupancyManager.Instance is null.");
            return;
        }

        bool registered = EntityOccupancyManager.Instance.TryRegister(currentFloor, gridPosition.x, gridPosition.y, gameObject);

        if (!registered)
        {
            Debug.LogWarning($"Player could not register occupancy at {gridPosition} on floor {currentFloor}.");
        }
    }

    public void TryMove(Vector2Int direction)
    {
        Vector2Int targetGrid = gridPosition + direction;

        if (!CanMoveTo(currentFloor, targetGrid.x, targetGrid.y))
            return;

        if (EntityOccupancyManager.Instance != null)
        {
            if (EntityOccupancyManager.Instance.IsOccupied(currentFloor, targetGrid.x, targetGrid.y))
                return;
        }

        StartCoroutine(MoveRoutine(targetGrid));
    }

    private bool CanMoveTo(int floor, int x, int y)
    {
        if (MapManager.Instance == null || MapManager.Instance.CurrentMap == null)
        {
            Debug.LogError("MapManager or CurrentMap is null.");
            return false;
        }

        if (TileDatabase.Instance == null)
        {
            Debug.LogError("TileDatabase.Instance is null.");
            return false;
        }

        MapData map = MapManager.Instance.CurrentMap;

        if (!map.IsValidPosition(floor, x, y))
            return false;

        MapTileData tileData = map.GetTile(floor, x, y);

        if (tileData == null)
            return false;

        if (tileData.groundTileId == 0)
            return false;

        TileDefinition groundTile = TileDatabase.Instance.GetTile(tileData.groundTileId);

        if (groundTile == null)
            return false;

        if (!groundTile.walkable)
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

    private IEnumerator MoveRoutine(Vector2Int targetGrid)
    {
        isMoving = true;

        Vector2Int oldGrid = gridPosition;

        if (EntityOccupancyManager.Instance != null)
        {
            bool movedInOccupancy = EntityOccupancyManager.Instance.Move(currentFloor, oldGrid, targetGrid, gameObject);

            if (!movedInOccupancy)
            {
                isMoving = false;
                yield break;
            }
        }

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = GridToWorld(targetGrid.x, targetGrid.y, currentFloor);

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
        gridPosition = targetGrid;
        isMoving = false;
    }

    private void OnDestroy()
    {
        if (EntityOccupancyManager.Instance != null)
        {
            EntityOccupancyManager.Instance.Unregister(currentFloor, gridPosition.x, gridPosition.y, gameObject);
        }
    }

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
}