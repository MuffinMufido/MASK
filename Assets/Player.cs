using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController p;
    public CharacterController pc;

    private Vector3 grvty;
    private float G = -9.8f * 2.0f;

    private float sprinting_speed = 5f;
    private float move_speed = 3.5f;
    private float current_speed;

    private bool is_grounded;

    public float jump_height = 2;
    public float gravity_scale = 5;

    private float xRotation;
    public float xSensitivity = 60;
    public float ySensitivity = 60;

    public Camera cam;
    private bool sprinting = false;

    public float coyoteTime = 0.3f;
    private float coyoteTimer;

    private bool jumpRequested = false;

    void Awake()
    {
        p = new PlayerController();
        pc = GetComponent<CharacterController>();
    }

    void OnEnable() => p.Enable();
    void OnDisable() => p.Disable();

    void Update()
    {
        current_speed = sprinting ? sprinting_speed : move_speed;

        is_grounded = pc.isGrounded;
        if (is_grounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;
        if (p.Player.Jump.triggered)
            jumpRequested = true;
    }

    void FixedUpdate()
    {
        Vector2 input = p.Player.Move.ReadValue<Vector2>();
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * input.y + camRight * input.x;
        pc.Move(moveDir * current_speed * Time.fixedDeltaTime);

        if (jumpRequested && (is_grounded || coyoteTimer > 0f))
        {
            grvty.y = Mathf.Sqrt(jump_height * -2f * G);
            jumpRequested = false;
            coyoteTimer = 0f;
        }

        grvty.y += G * Time.fixedDeltaTime;

        if (is_grounded && grvty.y < 0)
            grvty.y = -2;

        pc.Move(grvty * Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
        Vector2 look = p.Player.Look.ReadValue<Vector2>();
        xRotation -= look.y * ySensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.Rotate(Vector3.up * look.x * xSensitivity * Time.deltaTime);
    }
}
