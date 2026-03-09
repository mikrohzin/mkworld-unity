using System;

[Serializable]
public class MapTileData
{
    public int groundTileId;
    public int objectTileId;
    public int blockerTileId;

    public MapTileData()
    {
        groundTileId = 0;
        objectTileId = 0;
        blockerTileId = 0;
    }

    public MapTileData(int groundTileId, int objectTileId = 0, int blockerTileId = 0)
    {
        this.groundTileId = groundTileId;
        this.objectTileId = objectTileId;
        this.blockerTileId = blockerTileId;
    }

    public bool IsEmpty()
    {
        return groundTileId == 0 && objectTileId == 0 && blockerTileId == 0;
    }
}