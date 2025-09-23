using UnityEngine;

public class OneWayWall : MonoBehaviour
{
    public Vector2 blockDir = Vector2.right; // normalized
    void OnCollisionStay2D(Collision2D c)
    {
        var rb = c.rigidbody; if (!rb) return;
        if (Vector2.Dot(rb.velocity.normalized, blockDir) > 0.7f) rb.velocity = Vector2.zero;
    }
}
