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
  private bool is_grouneded;
  
  public float jump_height = 2;
  public float gravity_scale = 5;


  private float xRotation;
  public float xSensitivity = 60;
  public float ySensitivity = 60;

  public Camera cam;

  private bool sprinting = false;

  private Rigidbody held_object = null;
  private float held_distance = 1;

  int layer7Mask = 1 << 7;
  
  void Awake(){
    p = new PlayerController();
    pc = GetComponent<CharacterController>();
  }
  void OnEnable(){
    p.Enable();
  }
  void OnDisable(){
    p.Disable();
  }


  void FixedUpdate(){
    Vector3 velocity = Vector3.zero;

    velocity.x = p.Player.Move.ReadValue<Vector2>().x;
    velocity.z = p.Player.Move.ReadValue<Vector2>().y;
    Vector3 camForward = cam.transform.forward;
    Vector3 camRight = cam.transform.right;

    camForward.y = 0;
    camRight.y = 0;
    camForward.Normalize();
    camRight.Normalize();
    Vector3 moveDirection = (camForward * velocity.z) + (camRight * velocity.x);

    pc.Move(moveDirection * current_speed * Time.deltaTime);
    grvty.y += G * Time.deltaTime;
    if(is_grouneded && grvty.y < 0){
      grvty.y = -2;
    }
    pc.Move(grvty * Time.deltaTime);

  }


  void LateUpdate(){
    // float mouseX = p.Player.Look.ReadValue<Vector2>().x;
    // float mouseY = p.Player.Look.ReadValue<Vector2>().y;
    //
    // xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
    // xRotation = Mathf.Clamp(xRotation, -80, 80);
    //
    // transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
  }

  void Update()
  {
    current_speed = move_speed;
    if(sprinting){
      current_speed = sprinting_speed;
    }

    is_grouneded = pc.isGrounded;
    float jump_force = Mathf.Sqrt(jump_height * -3 * (Physics.gravity.y * gravity_scale));
    if(p.Player.Jump.triggered){
      if(is_grouneded){
        grvty.y = Mathf.Sqrt(jump_height * -3.0f * G);
      }
    }
  }
}

