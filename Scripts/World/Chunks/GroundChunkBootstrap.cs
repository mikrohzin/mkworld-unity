using UnityEngine;

public class GroundChunkBootstrap : MonoBehaviour
{
    [Header("Render Settings")]
    public int targetFloor = 0;
    public bool fillFloorOnStart = true;
    public int fillGroundTileId = 1;

    [Header("References")]
    public Transform chunkContainer;

    void Start()
    {
        if (MapManager.Instance == null)
        {
            Debug.LogError("MapManager not found.");
            return;
        }

        if (MapManager.Instance.CurrentMap == null)
        {
            Debug.LogError("CurrentMap is null.");
            return;
        }

        if (fillFloorOnStart)
        {
            MapManager.Instance.FillFloorWithGround(targetFloor, fillGroundTileId);
        }

        GenerateGroundChunks();
    }

    public void GenerateGroundChunks()
    {
        MapData map = MapManager.Instance.CurrentMap;

        if (chunkContainer == null)
        {
            GameObject container = new GameObject("GroundChunks");
            chunkContainer = container.transform;
        }

        ClearChildren(chunkContainer);

        int chunkCountX = Mathf.CeilToInt((float)map.width / map.chunkSize);
        int chunkCountY = Mathf.CeilToInt((float)map.height / map.chunkSize);

        for (int cy = 0; cy < chunkCountY; cy++)
        {
            for (int cx = 0; cx < chunkCountX; cx++)
            {
                GameObject chunkObject = new GameObject($"GroundChunk_F{targetFloor}_{cx}_{cy}");
                chunkObject.transform.SetParent(chunkContainer);

                GroundChunkRenderer renderer = chunkObject.AddComponent<GroundChunkRenderer>();
                renderer.Initialize(targetFloor, cx, cy, map.chunkSize);
                renderer.RenderChunk();
            }
        }

        Debug.Log($"Generated ground chunks for floor {targetFloor}.");
    }

    private void ClearChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(parent.GetChild(i).gameObject);
        }
    }
}