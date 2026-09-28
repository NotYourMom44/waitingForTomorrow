using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 5f;
    [SerializeField] private Transform cameraTransform;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool canMove = true;
    private float cameraPitch = 0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (canMove)
        {
            HandleMovement();
            HandleJump();
            HandleMouseLook();
        }

        HandleGravity();
    }

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;

        if (!enabled)
        {
            velocity.x = 0f;
            velocity.z = 0f;
        }
    }

    private void HandleMovement()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed)
                input.x -= 1f;

            if (Keyboard.current.dKey.isPressed)
                input.x += 1f;

            if (Keyboard.current.sKey.isPressed)
                input.y -= 1f;

            if (Keyboard.current.wKey.isPressed)
                input.y += 1f;
        }

        Vector3 movement = new Vector3(
            input.x,
            0f,
            input.y
        );

        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        movement = transform.TransformDirection(movement);

        characterController.Move(
            movement * moveSpeed * Time.deltaTime
        );
    }

    private void HandleMouseLook()
    {
        if (Mouse.current == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX =
            mouseDelta.x * mouseSensitivity * Time.deltaTime;

        float mouseY =
            mouseDelta.y * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(
            cameraPitch,
            -65f,
            80f
        );

        if (cameraTransform != null)
        {
            cameraTransform.rotation =
                Quaternion.Euler(
                    cameraPitch,
                    transform.eulerAngles.y,
                    0f
                );
        }
    }

    private void HandleGravity()
    {
        if (characterController.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(
            velocity * Time.deltaTime
        );
    }

    private void HandleJump()
    {
        Debug.Log(
            "Grounded: " +
            characterController.isGrounded
        );

        if (characterController.isGrounded &&
            Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            velocity.y =
                Mathf.Sqrt(
                    jumpHeight * -2f * gravity
                );
        }
    }
}