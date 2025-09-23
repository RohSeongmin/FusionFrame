using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("Prefabs (assign after running PrefabAutoBuilder)")]
    public Camera groupCameraPrefab;          // Prefabs/Cameras/GroupCamera.prefab
    public Transform blobPrefab;              // Prefabs/Gameplay/Blob.prefab

    [Header("Level Settings")]
    public int startingLevel = 1;
    public int maxLevel = 10;

    // Auto-created singletons
    private MultiSplitManager2D splitManager;
    private LevelFlowManager levelFlow;
    private InputHub inputHub;

    void Awake()
    {
        // Ensure InputHub
        inputHub = FindFirstObjectByType<InputHub>();
        if (!inputHub)
        {
            var go = new GameObject("InputHub");
            inputHub = go.AddComponent<InputHub>();
        }

        // Ensure SplitManager
        splitManager = FindFirstObjectByType<MultiSplitManager2D>();
        if (!splitManager)
        {
            var go = new GameObject("SplitManager");
            splitManager = go.AddComponent<MultiSplitManager2D>();
        }

        // Ensure LevelFlowManager
        levelFlow = FindFirstObjectByType<LevelFlowManager>();
        if (!levelFlow)
        {
            var go = new GameObject("LevelFlow");
            levelFlow = go.AddComponent<LevelFlowManager>();
        }

        // Wire references if prefabs were assigned
        if (groupCameraPrefab) splitManager.cameraPrefab = groupCameraPrefab;
        if (groupCameraPrefab) levelFlow.groupCameraPrefab = groupCameraPrefab;
        if (blobPrefab)        levelFlow.blobPrefab        = blobPrefab;

        // Pass level limits
        levelFlow.startingLevel = startingLevel;
        levelFlow.maxLevel      = maxLevel;

        // Keep the scene with exactly one AudioListener: add one if missing.
        if (FindFirstObjectByType<AudioListener>() == null)
        {
            var mainCam = Camera.main;
            if (mainCam) mainCam.gameObject.AddComponent<AudioListener>();
            else
            {
                var camGo = new GameObject("MainCamera");
                var cam   = camGo.AddComponent<Camera>();
                cam.orthographic = true;
                camGo.AddComponent<AudioListener>();
            }
        }
    }
}
