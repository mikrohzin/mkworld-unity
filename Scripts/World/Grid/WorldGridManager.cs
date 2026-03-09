using System.Collections.Generic;
using UnityEngine;

public class WorldGridManager : MonoBehaviour
{
    public static WorldGridManager Instance { get; private set; }

    [Header("Map Settings")]
    [SerializeField] private int mapWidth = 128;
    [SerializeField] private int mapHeight = 128;

    [Header("Visible Grid")]
    [SerializeField] private bool generateVisibleGrid = true;
    [SerializeField] private Material gridLineMaterial;
    [SerializeField] private float gridY = 0.05f;
    [SerializeField] private float lineWidth = 0.04f;
    [SerializeField] private Color gridColor = new Color(0f, 0f, 0f, 0.45f);

    private Dictionary<Vector2Int, WorldTile> tiles = new Dictionary<Vector2Int, WorldTile>();
    private Transform gridVisualRoot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        BuildGrid();
        RebuildVisibleGrid();
    }

    private void BuildGrid()
    {
        tiles.Clear();

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                tiles[pos] = new WorldTile(pos, true);
            }
        }
    }

    [ContextMenu("Rebuild Visible Grid")]
    public void RebuildVisibleGrid()
    {
        ClearVisibleGrid();

        if (!generateVisibleGrid)
            return;

        Material mat = GetOrCreateGridMaterial();

        GameObject root = new GameObject("GridVisual");
        root.transform.SetParent(transform, false);
        root.transform.localPosition = Vector3.zero;
        root.transform.localRotation = Quaternion.identity;
        root.transform.localScale = Vector3.one;
        gridVisualRoot = root.transform;

        // Linhas verticais
        for (int x = 0; x <= mapWidth; x++)
        {
            Vector3 start = new Vector3(x - 0.5f, gridY, -0.5f);
            Vector3 end = new Vector3(x - 0.5f, gridY, mapHeight - 0.5f);
            CreateGridLine($"GridLine_V_{x}", start, end, mat);
        }

        // Linhas horizontais
        for (int y = 0; y <= mapHeight; y++)
        {
            Vector3 start = new Vector3(-0.5f, gridY, y - 0.5f);
            Vector3 end = new Vector3(mapWidth - 0.5f, gridY, y - 0.5f);
            CreateGridLine($"GridLine_H_{y}", start, end, mat);
        }
    }

    private void ClearVisibleGrid()
    {
        if (gridVisualRoot == null)
            return;

        Destroy(gridVisualRoot.gameObject);
        gridVisualRoot = null;
    }

    private Material GetOrCreateGridMaterial()
    {
        if (gridLineMaterial != null)
            return gridLineMaterial;

        Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
        if (shader == null)
        {
            shader = Shader.Find("Sprites/Default");
        }

        Material fallback = new Material(shader);
        fallback.name = "M_GridLine_Fallback";
        fallback.color = gridColor;
        return fallback;
    }

    private void CreateGridLine(string lineName, Vector3 start, Vector3 end, Material mat)
    {
        GameObject lineObj = new GameObject(lineName);
        lineObj.transform.SetParent(gridVisualRoot, false);
        lineObj.transform.localPosition = Vector3.zero;
        lineObj.transform.localRotation = Quaternion.identity;
        lineObj.transform.localScale = Vector3.one;

        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.useWorldSpace = false;
        lr.loop = false;
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        lr.material = mat;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.startColor = gridColor;
        lr.endColor = gridColor;

        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.receiveShadows = false;
        lr.alignment = LineAlignment.TransformZ;
        lr.textureMode = LineTextureMode.Stretch;
        lr.numCapVertices = 0;
        lr.numCornerVertices = 0;
        lr.sortingOrder = 10;
    }

    public bool HasTile(Vector2Int gridPos)
    {
        return tiles.ContainsKey(gridPos);
    }

    public WorldTile GetTile(Vector2Int gridPos)
    {
        if (tiles.TryGetValue(gridPos, out WorldTile tile))
            return tile;

        return null;
    }

    public bool IsWalkable(Vector2Int gridPos)
    {
        WorldTile tile = GetTile(gridPos);

        if (tile == null)
            return false;

        return tile.walkable;
    }

    public bool IsBlocked(Vector2Int gridPos)
    {
        WorldTile tile = GetTile(gridPos);

        if (tile == null)
            return true;

        return tile.IsBlocked();
    }

    public bool TrySetOccupant(Vector2Int gridPos, GameObject occupant)
    {
        WorldTile tile = GetTile(gridPos);

        if (tile == null)
            return false;

        if (tile.occupant != null)
            return false;

        tile.SetOccupant(occupant);
        return true;
    }

    public void ClearOccupant(Vector2Int gridPos, GameObject occupant)
    {
        WorldTile tile = GetTile(gridPos);

        if (tile == null)
            return;

        if (tile.occupant == occupant)
            tile.ClearOccupant();
    }

    public bool MoveOccupant(Vector2Int from, Vector2Int to, GameObject occupant)
    {
        WorldTile fromTile = GetTile(from);
        WorldTile toTile = GetTile(to);

        if (fromTile == null || toTile == null)
            return false;

        if (!toTile.walkable)
            return false;

        if (toTile.occupant != null)
            return false;

        if (fromTile.occupant == occupant)
            fromTile.ClearOccupant();

        toTile.SetOccupant(occupant);
        return true;
    }
}