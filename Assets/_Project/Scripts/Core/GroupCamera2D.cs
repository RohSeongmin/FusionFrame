using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GroupCamera2D : MonoBehaviour
{
    public List<Transform> targets = new();
    public float padding = 2f, minSize = 5f, smooth = 0.15f;

    Camera cam; Vector3 vel;

    void Awake(){ cam = GetComponent<Camera>(); }

    void LateUpdate()
    {
        if (targets.Count==0) return;
        Vector3 min = targets[0].position, max = min;
        for (int i=1;i<targets.Count;i++){ var p=targets[i].position; min=Vector3.Min(min,p); max=Vector3.Max(max,p); }

        Vector3 center = (min+max)*0.5f;
        Vector3 goal = new(center.x, center.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, goal, ref vel, smooth);

        float dx = (max.x-min.x)+padding, dy=(max.y-min.y)+padding;
        float sizeX = dx*0.5f/cam.aspect, sizeY = dy*0.5f;
        float target = Mathf.Max(minSize, sizeX, sizeY);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, target, 1f - Mathf.Exp(-8f*Time.deltaTime));
    }
}
