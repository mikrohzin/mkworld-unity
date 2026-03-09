using UnityEngine;

public class Chunk : MonoBehaviour
{
    public int chunkX;
    public int chunkZ;
    public int chunkSize = 16;
    public GameObject groundPrefab;

    public void Generate()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                int worldX = chunkX * chunkSize + x;
                int worldZ = chunkZ * chunkSize + z;

                Vector3 position = new Vector3(worldX, 0f, worldZ);

                GameObject tile = Instantiate(groundPrefab, position, Quaternion.identity, transform);
                tile.name = $"Tile_{worldX}_{worldZ}";
            }
        }
    }
}