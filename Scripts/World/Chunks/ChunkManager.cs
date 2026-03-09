using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [Header("Map Settings")]
    public int mapSize = 128;
    public int chunkSize = 16;

    [Header("References")]
    public GameObject chunkPrefab;
    public GameObject groundPrefab;
    public Transform chunkContainer;

    void Start()
    {
        GenerateWorld();
    }

    public void GenerateWorld()
    {
        if (chunkPrefab == null)
        {
            Debug.LogError("Chunk Prefab não foi definido.");
            return;
        }

        if (groundPrefab == null)
        {
            Debug.LogError("Ground Prefab não foi definido.");
            return;
        }

        if (chunkContainer == null)
        {
            Debug.LogError("Chunk Container não foi definido.");
            return;
        }

        int chunksPerAxis = mapSize / chunkSize;

        for (int x = 0; x < chunksPerAxis; x++)
        {
            for (int z = 0; z < chunksPerAxis; z++)
            {
                GameObject chunkObj = Instantiate(chunkPrefab, chunkContainer);
                chunkObj.name = $"Chunk_{x}_{z}";

                Chunk chunk = chunkObj.GetComponent<Chunk>();
                chunk.chunkX = x;
                chunk.chunkZ = z;
                chunk.chunkSize = chunkSize;
                chunk.groundPrefab = groundPrefab;

                chunk.Generate();
            }
        }
    }
}