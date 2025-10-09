using UnityEngine;

[System.Serializable]
public class TileData
{
    public int x;
    public int y;
    public TileType type;
}

[System.Serializable]
public class GridData
{
    public int width;
    public int height;
    public TileData[] tiles;
}
