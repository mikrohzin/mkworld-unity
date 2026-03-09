using UnityEngine;

[CreateAssetMenu(fileName = "TileDefinition", menuName = "Tibia3D/Tile Definition")]
public class TileDefinition : ScriptableObject
{
    [Header("Identification")]
    public int id;
    public string tileName;

    [Header("Visual")]
    public GameObject groundPrefab;
    public GameObject objectPrefab;

    [Header("Gameplay")]
    public bool walkable = true;
    public bool blocksSight = false;
    public bool occupiesTile = false;

    [Header("Height")]
    public float height = 0f;

    [Header("Tags")]
    public string[] tags;
}