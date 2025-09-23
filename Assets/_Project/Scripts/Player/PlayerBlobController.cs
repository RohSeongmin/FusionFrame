using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBlobController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 6f;
    [Range(0.1f,1f)] public float stickSpeedScale = 0.6f;

    [Header("Sticky Internal Borders (viewport units)")]
    public float edgeEnter = 0.02f;
    public float edgeExit  = 0.03f;

    Rigidbody2D rb;
    Vector2 input; Camera owningCam;
    enum Stick { None, Vertical, Horizontal }
    Stick stick = Stick.None; int stickSign = 0;

    public void SetInput(Vector2 v){ input = (v.sqrMagnitude>1f) ? v.normalized : v; }

    void Awake(){ rb = GetComponent<Rigidbody2D>(); }

    void Update()
    {
        if (owningCam==null || !owningCam.gameObject.activeInHierarchy) owningCam = FindOwningCamera();
        if (owningCam) UpdateStick(owningCam);
        else stick = Stick.None;
    }

    void FixedUpdate()
    {
        Vector2 v;
        if (stick==Stick.None) v = input*moveSpeed;
        else if (stick==Stick.Vertical)   v = new Vector2(0f, input.y*moveSpeed*stickSpeedScale);
        else                               v = new Vector2(input.x*moveSpeed*stickSpeedScale, 0f);
        rb.velocity = v;
    }

    Camera FindOwningCamera()
    {
        var all = Object.FindObjectsByType<GroupCamera2D>(FindObjectsSortMode.None);
        foreach (var gc in all)
        {
            if (!gc.gameObject.activeInHierarchy) continue;
            if (gc.targets != null && gc.targets.Contains(transform)) return gc.GetComponent<Camera>();
        }
        return null;
    }

    void UpdateStick(Camera cam)
    {
        var vp = cam.WorldToViewportPoint(transform.position);
        if (vp.z < 0f){ stick=Stick.None; return; }

        Rect r = cam.rect;
        bool hasL = r.xMin>0f, hasR = r.xMax<1f, hasB = r.yMin>0f, hasT = r.yMax<1f;

        // release first
        if (stick==Stick.Vertical)
        {
            if ((stickSign<0 && vp.x>edgeExit) || (stickSign>0 && vp.x<1f-edgeExit) ||
                (stickSign<0 && input.x>0f)     || (stickSign>0 && input.x<0f))
                stick = Stick.None;
        }
        else if (stick==Stick.Horizontal)
        {
            if ((stickSign<0 && vp.y>edgeExit) || (stickSign>0 && vp.y<1f-edgeExit) ||
                (stickSign<0 && input.y>0f)     || (stickSign>0 && input.y<0f))
                stick = Stick.None;
        }
        if (stick!=Stick.None) return;

        // enter
        if (hasL && vp.x<=edgeEnter && input.x<0f){ stick=Stick.Vertical; stickSign=-1; return; }
        if (hasR && (1f-vp.x)<=edgeEnter && input.x>0f){ stick=Stick.Vertical; stickSign=+1; return; }
        if (hasB && vp.y<=edgeEnter && input.y<0f){ stick=Stick.Horizontal; stickSign=-1; return; }
        if (hasT && (1f-vp.y)<=edgeEnter && input.y>0f){ stick=Stick.Horizontal; stickSign=+1; return; }
    }
}
