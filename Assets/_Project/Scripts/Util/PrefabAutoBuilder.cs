#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;

public static class PrefabAutoBuilder
{
    [MenuItem("Tools/Build Jam Prefabs")]
    public static void BuildAll()
    {
        string root = "Assets/_Project";
        EnsureDir($"{root}/Prefabs/Cameras");
        EnsureDir($"{root}/Prefabs/Gameplay");
        EnsureDir($"{root}/Materials");

        // Ensure required gameplay scripts exist (light guard)
        RequireScript<PlayerBlobController>();
        RequireScript<WallSlideHelper>();
        RequireScript<BlobHealth>();
        RequireScript<GroupCamera2D>();
        RequireScript<MultiSplitManager2D>();
        RequireScript<InputHub>();
        RequireScript<LevelFlowManager>();

        // Physics material for walls
        var zeroFric = CreateZeroFriction($"{root}/Materials/ZeroFrictionWall.mat2d");

        // GroupCamera.prefab
        var groupCamPrefab = CreateGroupCamera($"{root}/Prefabs/Cameras/GroupCamera.prefab");

        // Blob.prefab
        var blobPrefab = CreateBlob($"{root}/Prefabs/Gameplay/Blob.prefab");

        // Wall.prefab
        var wallPrefab = CreateWall($"{root}/Prefabs/Gameplay/Wall.prefab", zeroFric);

        // Hazard.prefab
        var hazardPrefab = CreateHazard($"{root}/Prefabs/Gameplay/Hazard.prefab");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("PrefabAutoBuilder: Created GroupCamera, Blob, Wall, Hazard, and ZeroFrictionWall.");
        // Ping assets
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(groupCamPrefab));
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(blobPrefab));
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(wallPrefab));
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(hazardPrefab));
    }

    static void EnsureDir(string path)
    {
        if (!AssetDatabase.IsValidFolder(path))
        {
            var parent = Path.GetDirectoryName(path).Replace("\\", "/");
            var name = Path.GetFileName(path);
            AssetDatabase.CreateFolder(parent, name);
        }
    }

    static void RequireScript<T>() where T : MonoBehaviour
    {
        var guids = AssetDatabase.FindAssets($"t:Script {typeof(T).Name}");
        if (guids.Length == 0)
        {
            Debug.LogWarning($"PrefabAutoBuilder: Could not find script {typeof(T).Name}. Make sure it exists.");
        }
    }

    static string CreateGroupCamera(string path)
    {
        var go = new GameObject("GroupCamera");
        var cam = go.AddComponent<Camera>();
        cam.orthographic = true;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        cam.nearClipPlane = -50f; // generous for 2D
        cam.farClipPlane = 50f;
        go.AddComponent<GroupCamera2D>();
        // Do NOT add AudioListener; keep one in scene only.

        var prefab = PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
        return path;
    }

    static PhysicsMaterial2D CreateZeroFriction(string path)
    {
        var mat = new PhysicsMaterial2D("ZeroFrictionWall")
        {
            friction = 0f,
            bounciness = 0f
        };
        AssetDatabase.CreateAsset(mat, path);
        return mat;
    }

    // Replace your existing CreateBlob method with this one:
    static string CreateBlob(string path)
    {
        // Skip if already exists (so you don't overwrite a customized prefab)
        if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
        {
            Debug.Log($"Skipped: {path} already exists.");
            return path;
        }

        var go = new GameObject("Blob");

        // Visual (no sprite assigned here to avoid Texture->Sprite issues)
        var sr = go.AddComponent<SpriteRenderer>();
        // NOTE: With no sprite assigned, this won't render until you drop in a sprite later.
        // The color will apply once you assign a sprite.
        sr.color = new Color(0.2f, 0.9f, 1f, 1f);

        // Physics
        var rb = go.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        var col = go.AddComponent<CircleCollider2D>();
        col.radius = 0.5f;

        // Scripts
        var ctrl = go.AddComponent<PlayerBlobController>();
        ctrl.moveSpeed = 6f;
        ctrl.stickSpeedScale = 0.6f;
        ctrl.edgeEnter = 0.02f;
        ctrl.edgeExit = 0.03f;

        var slide = go.AddComponent<WallSlideHelper>();
        slide.slideBoost = 1.0f;

        var hp = go.AddComponent<BlobHealth>();
        hp.maxHealth = 3;
        hp.invuln = 0.5f;

        var prefab = PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
        return path;
    }


    static string CreateWall(string path, PhysicsMaterial2D mat)
    {
        var go = new GameObject("Wall");
        go.tag = "Wall";

        var sr = go.AddComponent<SpriteRenderer>();
        sr.color = new Color(0.15f, 0.15f, 0.18f, 1f);

        var col = go.AddComponent<BoxCollider2D>();
        col.sharedMaterial = mat;
        col.size = new Vector2(3f, 0.5f); // default plank

        // Static collider by default (no RigidBody2D needed)

        var prefab = PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
        return path;
    }

    static string CreateHazard(string path)
    {
        var go = new GameObject("Hazard");
        go.tag = "Hazard";

        var sr = go.AddComponent<SpriteRenderer>();
        sr.color = new Color(1f, 0.25f, 0.25f, 1f);

        var col = go.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size = new Vector2(1.2f, 1.2f);

        var prefab = PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
        return path;
    }
}
#endif
