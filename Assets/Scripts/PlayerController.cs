using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CameraRig cameraRig;

    public float speed = 5f;
    public float jumpHeight = 1.3f;
    public float gravity = -20f;
    public float turnSmoothTime = 0.08f;

    CharacterController CharacterController;
    float verticalVelocity;
    float turnVelocity;
    Vector3 spawnPosition;

    void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        spawnPosition = transform.position;
    }

    void Update()
    {
        GameManager gm = GameManager.Instance;
        bool canControl = gm == null || gm.State == GameState.Playing;
        Vector2 input = canControl ? GameInput.Move : Vector2.zero;

        if (CharacterController.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;

        if (canControl && input.sqrMagnitude > 0.01f)
        {
            float yaw = cameraRig != null ? cameraRig.Yaw : transform.eulerAngles.y;
            Vector3 dir = Quaternion.Euler(0f, yaw, 0f) * new Vector3(input.x, 0f, input.y);

            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            CharacterController.Move(dir * speed * Time.deltaTime);
        }

        if (canControl && CharacterController.isGrounded && GameInput.JumpDown)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime;
        CharacterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
        if (transform.position.y < -15f) Teleport(spawnPosition); 
    }

    public void Teleport(Vector3 position)
    {
        CharacterController.enabled = false;
        transform.position = position;
        CharacterController.enabled = true;
        verticalVelocity = 0f;
    }
}
