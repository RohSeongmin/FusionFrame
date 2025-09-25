using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GridManager gridManager;
    public int x, y; // current tile coords

    [HideInInspector] public bool canMove = true; // Disable movement during victory/death

    void Start()
    {
        transform.position = gridManager.GetWorldPosition(x, y);
    }

    void Update()
    {
        if (!canMove) return; // Stop movement if movement is disabled

        Vector2Int moveDir = Vector2Int.zero;

        // WASD keys
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) moveDir = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) moveDir = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) moveDir = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) moveDir = Vector2Int.right;

        if (moveDir != Vector2Int.zero)
        {
            int targetX = x + moveDir.x;
            int targetY = y + moveDir.y;

            if (gridManager.IsWalkable(targetX, targetY))
            {
                x = targetX;
                y = targetY;
                transform.position = gridManager.GetWorldPosition(x, y);
            }
        }
    }

    // Helper to get the Tile the player is currently standing on
    public Tile CurrentTile()
    {
        return gridManager.GetTile(x, y);
    }
}
