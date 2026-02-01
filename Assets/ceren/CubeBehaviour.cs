using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CubeBehaviorNewInput : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Enable this cube to move along a path.")]
    public bool canMove = false;
    public bool canScale = false;

    private bool canHide = true;
    [Tooltip("Speed of movement along the path.")]
    public float moveSpeed = 2f;

    private List<Transform> pathPoints = new List<Transform>(); // automatically filled with children
    private int currentTargetIndex = 0;
    private bool isMoving = false;

    [Header("Scale Settings")]
    private Vector3 originalScale;
    private Rigidbody rb;
    private float originalMass;

    private bool hidden = false;
    private bool scaledUp = false;

    [Header("Input")]
    public InputActionAsset inputActions;
    public string hideShowActionName = "HideShow";
    public string scaleActionName = "Scale";
    public string moveActionName = "Move";

    private InputAction hideShowAction;
    private InputAction scaleAction;
    private InputAction moveAction;

    private void Awake()
    {
        originalScale = transform.localScale;

        rb = GetComponent<Rigidbody>();
        if (rb != null)
            originalMass = rb.mass;

        var map = inputActions.FindActionMap("CubeActions");






        if (canHide)
        {
            hideShowAction = map.FindAction(hideShowActionName);
            hideShowAction.performed += ctx => ToggleHideShow();

        }
        if (canScale)
        {
            scaleAction = map.FindAction(scaleActionName);
            scaleAction.performed += ctx => ToggleScale();
        }

        if (canMove)
        {
            foreach (Transform child in transform)
                pathPoints.Add(child);
      
            foreach (Transform t in pathPoints)
                t.SetParent(null);

            moveAction = map.FindAction(moveActionName);
            moveAction.performed += ctx => StartMove();
        }
    }

    private void OnEnable()
    {
        if (hideShowAction != null) hideShowAction.Enable();
        if (scaleAction != null) scaleAction.Enable();
        if (moveAction != null) moveAction.Enable();
    }

    private void OnDisable()
    {

          if (hideShowAction != null) hideShowAction.Disable();
        if (scaleAction != null) scaleAction.Disable();
        if (moveAction != null) moveAction.Disable();
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

    private void StartMove()
    {
        if (!canMove || pathPoints.Count == 0)
            return;
           isMoving = !isMoving;
        if (isMoving && currentTargetIndex >= pathPoints.Count)
        {
            currentTargetIndex = 0;
        }
    }

    private void Update()
    {
        if (!canMove || !isMoving || pathPoints.Count == 0) return;

    Transform target = pathPoints[currentTargetIndex];


    transform.position = Vector3.MoveTowards(
        transform.position,
        target.position,
        moveSpeed * Time.deltaTime
    );


    if (Vector3.Distance(transform.position, target.position) < 0.01f)
    {
        currentTargetIndex++;
        if (currentTargetIndex >= pathPoints.Count)
            currentTargetIndex = 0;
    }
  
    }

    // Optional: visualize path in Scene view
    private void OnDrawGizmos()
    {
        if (pathPoints == null || pathPoints.Count == 0) return;
        Gizmos.color = Color.yellow;
        for (int i = 0; i < pathPoints.Count - 1; i++)
            Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
    }
}
