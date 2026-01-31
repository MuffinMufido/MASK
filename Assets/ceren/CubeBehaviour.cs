using UnityEngine;

[System.Flags]
public enum CubeFunctionality
{
    None = 0,
    HideShow = 1 << 0, // 0b01
    Scale = 1 << 1     // 0b10
}

public class CubeBehavior : MonoBehaviour
{
    [Header("Settings")]
    public CubeFunctionality functionalityMask = CubeFunctionality.HideShow | CubeFunctionality.Scale;

    [Header("Scaling Settings")]
    [Range(0.1f, 10f)]
    public float transitionSpeed = 5f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    private Rigidbody rb;
    private float originalMass;
    private float targetMass;

    private bool hidden = false;
    private bool scaledUp = false;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;

        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            originalMass = rb.mass;
            targetMass = originalMass;
        }
    }

    void Update()
    {
    
        if (Input.GetKeyDown(KeyCode.Alpha1) && (functionalityMask & CubeFunctionality.HideShow) != 0)
        {
            hidden = !hidden;

            Renderer rend = GetComponent<Renderer>();
            Collider col = GetComponent<Collider>();

            if (rend != null) rend.enabled = !hidden;
            if (col != null) col.enabled = !hidden;
        }


        if (Input.GetKeyDown(KeyCode.Alpha2) && (functionalityMask & CubeFunctionality.Scale) != 0)
        {
            scaledUp = !scaledUp;
            targetScale = scaledUp ? originalScale * 2f : originalScale;

            if (rb != null)
                targetMass = scaledUp ? originalMass * 8f : originalMass;
        }

        // --- Smooth transition ---
        if ((functionalityMask & CubeFunctionality.Scale) != 0)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);

            if (rb != null)
                rb.mass = Mathf.Lerp(rb.mass, targetMass, Time.deltaTime * transitionSpeed);
        }
    }
}
