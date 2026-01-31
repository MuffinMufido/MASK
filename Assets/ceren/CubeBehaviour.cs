using UnityEngine;
using UnityEngine.InputSystem;

public class CubeBehaviorNewInput : MonoBehaviour
{
    private Vector3 originalScale;
    private Rigidbody rb;
    private float originalMass;

    private bool hidden = false;
    private bool scaledUp = false;

    public InputActionAsset inputActions;
    private InputAction hideShowAction;
    private InputAction scaleAction;

    private void Awake()
    {
        originalScale = transform.localScale;

        rb = GetComponent<Rigidbody>();
        if (rb != null)
            originalMass = rb.mass;

        var map = inputActions.FindActionMap("CubeActions");
        hideShowAction = map.FindAction("HideShow");
        scaleAction = map.FindAction("Scale");

        hideShowAction.performed += ctx => ToggleHideShow();
        scaleAction.performed += ctx => ToggleScale();
    }

    private void OnEnable()
    {
        hideShowAction.Enable();
        scaleAction.Enable();
    }

    private void OnDisable()
    {
        hideShowAction.Disable();
        scaleAction.Disable();
    }

    private void ToggleHideShow()
    {
        hidden = !hidden;

        Renderer rend = GetComponent<Renderer>();
        Collider col = GetComponent<Collider>();

        if (rend != null) rend.enabled = !hidden;
        if (col != null) col.enabled = !hidden;
    }

    private void ToggleScale()
    {
        scaledUp = !scaledUp;

        transform.localScale = scaledUp ? originalScale * 2f : originalScale;

        if (rb != null)
            rb.mass = scaledUp ? originalMass * 8f : originalMass;
    }
}
