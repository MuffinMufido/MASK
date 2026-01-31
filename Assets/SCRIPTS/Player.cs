using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController p;
    public CharacterController pc;

    private Vector3 grvty;
    private float G = -9.8f * 2.0f;

    private float sprinting_speed = 5f;
    private float move_speed = 10f;
    private float current_speed;

    private bool is_grounded;

    public float jump_height = 2;
    public float gravity_scale = 5;

    private float xRotation;
    public float xSensitivity = 60;
    public float ySensitivity = 60;

    public Camera cam;
    public Camera cam_big;

    private bool sprinting = false;

    public float coyoteTime = 0.00f;
    private float coyoteTimer;

    private bool jumpRequested = false;
    private float jumpBufferTime = 0f;

    public bool big_guy = false;

    void Awake()
    {
        p = new PlayerController();
        pc = GetComponent<CharacterController>();
        pc.enabled = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        p.Player.Big.performed += ctx => ToggleBigGuy();
        
    }

    public void Init(){
      pc.enabled = true;
    }

    void OnEnable() => p.Enable();
    void OnDisable() => p.Disable();

    void Update()
    {

        if(big_guy){return;}
        current_speed = move_speed;

        is_grounded = pc.isGrounded;
        if (is_grounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;
        if(jumpBufferTime >= 0){
          jumpBufferTime -= Time.deltaTime;

        }
        if (p.Player.Jump.triggered)
          jumpBufferTime = 0.1f;

    }

    void FixedUpdate()
    {
        if(big_guy){return;}
        Vector2 input = p.Player.Move.ReadValue<Vector2>();
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * input.y + camRight * input.x;
        pc.Move(moveDir * current_speed * Time.fixedDeltaTime);

        if (jumpBufferTime >= 0 && (is_grounded || coyoteTimer > 0f))
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

        float mouseX = p.Player.Look.ReadValue<Vector2>().x;
        float mouseY = p.Player.Look.ReadValue<Vector2>().y;
        
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80, 80);
        if(big_guy){
          cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }
        else{
          cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
          transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
        }

    }

    void ToggleBigGuy(){
      cam.enabled = !cam.enabled;
      cam_big.enabled = !cam_big.enabled;
      big_guy = !big_guy;
    }
}
