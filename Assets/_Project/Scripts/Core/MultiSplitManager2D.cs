using System.Collections.Generic;
using UnityEngine;

public class MultiSplitManager2D : MonoBehaviour
{
    [Header("Players")]
    public List<Transform> players = new();

    [Header("Hysteresis")]
    public float joinDistance = 1.0f;   // merge if <= this
    public float splitDistance = 2.0f;  // allow split if >= this

    [Header("Cameras")]
    public Camera cameraPrefab;
    public int maxCameras = 8;
    public bool preferLeftRightForTwo = true;

    public IReadOnlyList<List<Transform>> Groups => _currGroups;
    public int ActiveGroupCount => _activeCount;

    readonly List<Camera> _pool = new();
    readonly List<List<Transform>> _prevGroups = new();
    readonly List<List<Transform>> _currGroups = new();
    int _activeCount;

    void Start(){ EnsurePool(); }

    void Update()
    {
        BuildGroupsWithHysteresis();
        AssignCameras();
        LayoutViewports();
    }

    void EnsurePool()
    {
        while (_pool.Count < maxCameras)
        {
            var cam = Instantiate(cameraPrefab, transform);
            cam.gameObject.SetActive(false);
            cam.orthographic = true;
            if (!cam.GetComponent<GroupCamera2D>()) cam.gameObject.AddComponent<GroupCamera2D>();
            _pool.Add(cam);
        }
    }

    // --- grouping ---
    void BuildGroupsWithHysteresis()
    {
        _currGroups.Clear();
        var comps = ConnectedComponents(players, joinDistance);

        if (_prevGroups.Count == 0) _currGroups.AddRange(comps);
        else
        {
            var locked = new HashSet<Transform>();
            foreach (var g in _prevGroups)
            {
                // Did g split?
                var parts = new List<List<Transform>>();
                foreach (var c in comps)
                    for (int i=0;i<c.Count;i++) if (g.Contains(c[i])) { parts.Add(c); break; }

                if (parts.Count > 1 && !AllowSplit(g))
                {
                    _currGroups.Add(new List<Transform>(g));
                    foreach (var t in g) locked.Add(t);
                }
            }
            foreach (var c in comps)
            {
                bool skip=false; foreach (var t in c) if (locked.Contains(t)) { skip=true; break; }
                if (!skip) _currGroups.Add(c);
            }
        }

        _prevGroups.Clear();
        foreach (var g in _currGroups) _prevGroups.Add(new List<Transform>(g));
    }

    bool AllowSplit(List<Transform> g)
    {
        float maxd = 0f;
        for (int i=0;i<g.Count;i++)
        for (int j=i+1;j<g.Count;j++)
            maxd = Mathf.Max(maxd, Vector2.Distance(g[i].position, g[j].position));
        return maxd >= splitDistance;
    }

    static List<List<Transform>> ConnectedComponents(List<Transform> nodes, float thr)
    {
        var res = new List<List<Transform>>();
        int n = nodes.Count; var vis = new bool[n];
        for (int i=0;i<n;i++)
        {
            if (nodes[i]==null || vis[i]) continue;
            var comp = new List<Transform>(); var st = new Stack<int>();
            st.Push(i); vis[i]=true;
            while (st.Count>0)
            {
                int u=st.Pop(); var tu=nodes[u]; comp.Add(tu);
                for (int v=0; v<n; v++)
                {
                    if (vis[v] || nodes[v]==null) continue;
                    if (Vector2.Distance(tu.position, nodes[v].position) <= thr)
                    { vis[v]=true; st.Push(v); }
                }
            }
            res.Add(comp);
        }
        return res;
    }

    // --- cameras ---
    void AssignCameras()
    {
        EnsurePool();
        foreach (var c in _pool) c.gameObject.SetActive(false);
        int g = Mathf.Min(_currGroups.Count, _pool.Count);
        _activeCount = g;

        for (int i=0;i<g;i++)
        {
            var cam = _pool[i]; cam.gameObject.SetActive(true);
            var gc = cam.GetComponent<GroupCamera2D>();
            gc.targets.Clear(); gc.targets.AddRange(_currGroups[i]);
            cam.rect = new Rect(0,0,1,1);
            cam.depth = 10 + i;
        }
    }

    void LayoutViewports()
    {
        int g=0; for (int i=0;i<_pool.Count;i++) if (_pool[i].gameObject.activeSelf) g++;
        if (g==0) return;

        if (g==2 && preferLeftRightForTwo)
        {
            _pool[0].rect = new Rect(0f,0f,0.5f,1f);
            _pool[1].rect = new Rect(0.5f,0f,0.5f,1f);
            return;
        }

        int rows = Mathf.CeilToInt(Mathf.Sqrt(g));
        int cols = Mathf.CeilToInt((float)g / rows);
        float w = 1f/cols, h = 1f/rows;

        for (int i=0;i<g;i++)
        {
            int r = i/cols, c = i%cols;
            int rInv = (rows-1)-r;
            _pool[i].rect = new Rect(c*w, rInv*h, w, h);
        }
    }
}
