using UnityEngine;

public class BlobHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float invuln = 0.5f;
    int hp; float lastHit = -999f;

    void Awake(){ hp = maxHealth; }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Hazard")) Hit(1);
    }

    public void Hit(int dmg)
    {
        if (Time.time - lastHit < invuln) return;
        hp -= dmg; lastHit = Time.time;
        if (hp <= 0) Die();
    }

    void Die()
    {
        gameObject.SetActive(false);
        // Optional: if all dead, notify LevelFlowManager
        var all = FindObjectsByType<BlobHealth>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        bool anyAlive = false; foreach (var b in all) if (b.gameObject.activeInHierarchy) { anyAlive=true; break; }
        if (!anyAlive) FindFirstObjectByType<LevelFlowManager>()?.OnAllBlobsDead();
    }
}
