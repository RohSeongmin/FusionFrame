using UnityEngine;

public enum TileType
{
    Floor,
    Wall,
    Spike,
    Goal
}

public class Tile : MonoBehaviour
{
    public int x, y;
    [SerializeField] private TileType type = TileType.Floor;
    private SpriteRenderer sr;

    [Header("Tile Sprites")]
    public Sprite floorSprite;
    public Sprite wallSprite;
    public Sprite spikeSprite;
    public Sprite goalSprite;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Init(int x, int y, TileType type)
    {
        this.x = x;
        this.y = y;
        SetType(type);
    }

    public void SetType(TileType newType)
    {
        type = newType;

        if (sr == null) return;

        // Assign sprite based on tile type
        switch (type)
        {
            case TileType.Floor: sr.sprite = floorSprite; break;
            case TileType.Wall: sr.sprite = wallSprite; break;
            case TileType.Spike: sr.sprite = spikeSprite; break;
            case TileType.Goal: sr.sprite = goalSprite; break;
        }
    }

    public TileType GetTileType() => type;

    // Walkable unless it's a Wall
    public bool IsWalkable() => type != TileType.Wall;
}
