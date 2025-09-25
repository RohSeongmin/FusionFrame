using UnityEngine;

[System.Serializable]
public class TileOverride
{
    public int x;
    public int y;
    public TileType type = TileType.Floor;
}

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 10;
    public int height = 10;
    public GameObject tilePrefab;

    [Header("Tile Overrides (editable in Inspector)")]
    public TileOverride[] tiles; // assign manually in Inspector

    private Tile[,] grid;

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        grid = new Tile[width, height];

        float offsetX = width / 2f - 0.5f;
        float offsetY = height / 2f - 0.5f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPos = new Vector3(x - offsetX, y - offsetY, 0) + transform.position;

                GameObject tileObj = Instantiate(tilePrefab, worldPos, Quaternion.identity, transform);
                tileObj.name = $"Tile_{x}_{y}";

                Tile tile = tileObj.GetComponent<Tile>();

                // Check if a tile override exists
                TileOverride overrideData = System.Array.Find(tiles, t => t.x == x && t.y == y);
                TileType type = overrideData != null ? overrideData.type : TileType.Floor;

                tile.Init(x, y, type);
                grid[x, y] = tile;
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        float offsetX = width / 2f - 0.5f;
        float offsetY = height / 2f - 0.5f;

        return new Vector3(x - offsetX, y - offsetY, 0) + transform.position;
    }

    public bool IsWalkable(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return false;

        return grid[x, y].IsWalkable();
    }

    public Tile GetTile(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return null;

        return grid[x, y];
    }
}
