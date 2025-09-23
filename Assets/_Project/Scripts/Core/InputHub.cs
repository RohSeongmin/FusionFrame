using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class InputHub : MonoBehaviour
{
    public bool autoDiscover = true;
    public List<PlayerBlobController> blobs = new();

#if ENABLE_INPUT_SYSTEM
    InputAction moveAction;
#endif

    void Awake()
    {
#if ENABLE_INPUT_SYSTEM
        moveAction = new InputAction("Move", InputActionType.Value, expectedControlType: "Vector2");
        var wasd = moveAction.AddCompositeBinding("2DVector");
        wasd.With("Up","<Keyboard>/w").With("Down","<Keyboard>/s")
            .With("Left","<Keyboard>/a").With("Right","<Keyboard>/d");
        var arrows = moveAction.AddCompositeBinding("2DVector");
        arrows.With("Up","<Keyboard>/upArrow").With("Down","<Keyboard>/downArrow")
              .With("Left","<Keyboard>/leftArrow").With("Right","<Keyboard>/rightArrow");
        moveAction.Enable();
#endif
    }

    void LateUpdate()
    {
        if (autoDiscover)
        {
            blobs.Clear();
            blobs.AddRange(FindObjectsByType<PlayerBlobController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
        }
        Vector2 v;
#if ENABLE_INPUT_SYSTEM
        v = moveAction.ReadValue<Vector2>();
#else
        v = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#endif
        if (v.sqrMagnitude > 1f) v.Normalize();
        foreach (var b in blobs) if (b) b.SetInput(v);
    }
}
