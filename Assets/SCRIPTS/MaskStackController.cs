using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MaskStackController : MonoBehaviour
{
    [Header("Mask Assets (children of Man)")]
    public List<Transform> masks;
    // index 0 → key 1
    // index 1 → key 2
    // index 2 → key 3
    // index 3 → key 4

    [Header("Stack Start (assign empty GameObject here)")]
    public Transform stackStart;   // defines position + rotation of stack base

    [Header("Visual Stack")]
    public float stackYOffset = 0.15f;

    private Stack<Transform> stack = new Stack<Transform>();

    [Header("Input")]
    public InputActionAsset inputActions;
    public string actionMapName = "Player";

    private InputAction a1, a2, a3, a4;

    // ----------------------------------------------------

    private void Awake()
    {
        // Disable all masks at start
        foreach (var m in masks)
            m.gameObject.SetActive(false);

        var map = inputActions.FindActionMap(actionMapName);

        // KEEPING YOUR ACTION NAMES
        a1 = map.FindAction("mask1Action");
        a2 = map.FindAction("mask2Action");
        a3 = map.FindAction("mask3Action");
        a4 = map.FindAction("mask4Action");

        a1.performed += _ => ActivateAndStack(0);
        a2.performed += _ => ActivateAndStack(1);
        a3.performed += _ => ActivateAndStack(2);
        a4.performed += _ => ActivateAndStack(3);
    }

    private void OnEnable()
    {
        a1.Enable();
        a2.Enable();
        a3.Enable();
        a4.Enable();
    }

    private void OnDisable()
    {
        a1.Disable();
        a2.Disable();
        a3.Disable();
        a4.Disable();
    }

    // ----------------------------------------------------
    // CORE BEHAVIOR
    // ----------------------------------------------------

    private void ActivateAndStack(int index)
    {
        if (index < 0 || index >= masks.Count)
            return;

        Transform mask = masks[index];

        // If mask already in stack → move it to top
        if (stack.Contains(mask))
            RemoveFromStack(mask);

        mask.gameObject.SetActive(true);
        stack.Push(mask);

        UpdateVisualStack();
    }

    private void RemoveFromStack(Transform target)
    {
        Stack<Transform> temp = new Stack<Transform>();

        while (stack.Count > 0)
        {
            Transform m = stack.Pop();
            if (m != target)
                temp.Push(m);
        }

        while (temp.Count > 0)
            stack.Push(temp.Pop());
    }

    private void UpdateVisualStack()
    {
        if (stackStart == null)
        {
            Debug.LogWarning("StackStart is not assigned.");
            return;
        }

        int i = 0;

        foreach (Transform m in stack)
        {
            // Position: start point + up offset
            m.position =
                stackStart.position +
                stackStart.up * (i * stackYOffset);

            // Rotation: exactly match stack start
            m.rotation = stackStart.rotation;

            i++;
        }
    }
}
