using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFlowManager : MonoBehaviour
{
    [Header("Refs")]
    public MultiSplitManager2D splitManager;
    public Camera groupCameraPrefab;
    public Transform blobPrefab;

    [Header("Levels")]
    public int startingLevel = 1;
    public int maxLevel = 10;
    public float startDelay = 0.4f;
    public float winStableSec = 0.25f;
    public float winPause = 0.6f;

    [Header("Spawn")]
    public float radius = 10f;

    [Header("Tuning per level")]
    public AnimationCurve joinDist = AnimationCurve.Linear(1,1f,10,1f);
    public AnimationCurve splitDist = AnimationCurve.Linear(1,2f,10,2f);
    public AnimationCurve stickScale = AnimationCurve.Linear(1,0.6f,10,0.6f);

    int level;
    readonly List<Transform> spawned = new();

    void Start()
    {
        if (!splitManager) splitManager = FindFirstObjectByType<MultiSplitManager2D>();
        if (splitManager && groupCameraPrefab) splitManager.cameraPrefab = groupCameraPrefab;
        level = Mathf.Max(1, startingLevel);
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (level <= maxLevel)
        {
            yield return StartCoroutine(StartLevel(level));
            yield return StartCoroutine(WaitMerged());
            yield return new WaitForSeconds(winPause);
            level++;
        }
        Debug.Log("All levels complete!");
    }

    IEnumerator StartLevel(int L)
    {
        // cleanup
        foreach (var t in spawned) if (t) Destroy(t.gameObject);
        spawned.Clear();
        if (splitManager) splitManager.players.Clear();

        // tune
        if (splitManager)
        {
            splitManager.joinDistance  = joinDist.Evaluate(L);
            splitManager.splitDistance = splitDist.Evaluate(L);
        }

        int count = L + 1;
        float step = 360f / count;
        for (int i = 0; i < count; i++)
        {
            float ang = (i * step) * Mathf.Deg2Rad;
            Vector3 pos = new(Mathf.Cos(ang)*radius, Mathf.Sin(ang)*radius, 0);
            var blob = Instantiate(blobPrefab, pos, Quaternion.identity);
            spawned.Add(blob);

            var stick = blob.GetComponent<PlayerBlobController>();
            if (stick) stick.stickSpeedScale = stickScale.Evaluate(L);

            if (splitManager) splitManager.players.Add(blob);
        }
        yield return null;
        yield return new WaitForSeconds(startDelay);
        Debug.Log($"Level {L}: spawned {count} blobs");
    }

    IEnumerator WaitMerged()
    {
        float ok = 0f;
        while (true)
        {
            if (splitManager && splitManager.ActiveGroupCount == 1) ok += Time.deltaTime;
            else ok = 0f;
            if (ok >= winStableSec) yield break;
            yield return null;
        }
    }

    // optional: fail condition hook
    public void OnAllBlobsDead()
    {
        Debug.Log("All blobs dead — restarting level");
        StopAllCoroutines();
        StartCoroutine(Loop());
    }
}
