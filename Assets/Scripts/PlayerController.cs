using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 5f;
    [SerializeField] private Transform cameraTransform;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Footsteps")]
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private float walkStepInterval = 0.5f;
    [SerializeField] private float sprintStepInterval = 0.35f;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool canMove = true;
    private float cameraPitch = 0f;
    private float footstepTimer = 0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
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
        UpdateAnimation();
        HandleFootsteps();
    }

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;

        if (!enabled)
        {
            velocity.x = 0f;
            velocity.z = 0f;
            footstepTimer = 0f;

            if (footstepAudioSource != null)
            {
                footstepAudioSource.Stop();
            }

            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
            }
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

        bool isSprinting =
            Keyboard.current != null &&
            Keyboard.current.leftShiftKey.isPressed &&
            input.magnitude > 0f;

        float currentSpeed =
            isSprinting ? sprintSpeed : moveSpeed;

        characterController.Move(
            movement * currentSpeed * Time.deltaTime
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

    private void UpdateAnimation()
    {
        if (animator == null)
            return;

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

        bool hasMovementInput = input.magnitude > 0f;

        bool isSprinting =
            Keyboard.current != null &&
            Keyboard.current.leftShiftKey.isPressed &&
            hasMovementInput;

        float animationSpeed = 0f;

        if (hasMovementInput)
        {
            animationSpeed = isSprinting ? 2f : 1f;
        }

        animator.SetFloat("Speed", animationSpeed);
        animator.SetBool(
            "IsGrounded",
            characterController.isGrounded
        );
    }

    private void HandleFootsteps()
    {
        if (footstepAudioSource == null ||
            footstepClip == null ||
            !canMove ||
            !characterController.isGrounded)
        {
            footstepTimer = 0f;
            return;
        }

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

        bool hasMovementInput = input.magnitude > 0f;

        if (!hasMovementInput)
        {
            footstepTimer = 0f;
            return;
        }

        bool isSprinting =
            Keyboard.current != null &&
            Keyboard.current.leftShiftKey.isPressed;

        float stepInterval =
            isSprinting
                ? sprintStepInterval
                : walkStepInterval;

        footstepTimer += Time.deltaTime;

        if (footstepTimer >= stepInterval)
        {
            footstepAudioSource.PlayOneShot(footstepClip);
            footstepTimer = 0f;
        }
    }
}