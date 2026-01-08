using UnityEngine;

public class FreeFlyCamera : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 0.5f;
    public float fastMultiplier = 0.2f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 85f;

    float yaw;
    float pitch;
    public float acceleration = 12f;
    public float deceleration = 18f;

    Vector3 currentVelocity;


    void Start()
    {
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;       
    }

    

    void Update()
    {
        Look();
        Move();
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        float y = 0f;

        if (Input.GetKey(KeyCode.E)) y += 1f;
        if (Input.GetKey(KeyCode.Q)) y -= 1f;

        float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? fastMultiplier : 0.3f);
        Vector3 inputDir = new Vector3(x, y, z);
        inputDir = Vector3.ClampMagnitude(inputDir, 0.3f);

        Vector3 desiredVelocity = (transform.right * inputDir.x + transform.up * inputDir.y + transform.forward * inputDir.z) * speed;

        float rate = (inputDir.sqrMagnitude > 0.001f) ? acceleration : deceleration;

        currentVelocity = Vector3.MoveTowards(currentVelocity, desiredVelocity, rate * Time.deltaTime);

        transform.position += currentVelocity * Time.deltaTime;
    }
}
