using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WallSlideHelper : MonoBehaviour
{
    public float slideBoost = 1.0f; // >1 to exaggerate glide
    Rigidbody2D rb;

    void Awake(){ rb = GetComponent<Rigidbody2D>(); }

    void OnCollisionStay2D(Collision2D c)
    {
        if (!c.collider.CompareTag("Wall")) return;
        var n = c.contacts[0].normal;
        var t = new Vector2(-n.y, n.x).normalized;
        float proj = Vector2.Dot(rb.velocity, t);
        rb.velocity = t * proj * slideBoost;
    }
}
