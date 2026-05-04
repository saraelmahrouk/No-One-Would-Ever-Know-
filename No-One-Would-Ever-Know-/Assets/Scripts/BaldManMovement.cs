using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Camera")]
    public Camera playerCamera;

    [Header("Movement Speeds")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float crouchSpeed = 3f;

    [Header("Jump and Gravity")]
    public float jumpPower = 7f;
    public float gravity = 20f;

    [Header("Mouse Look")]
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    [Header("Crouch Settings")]
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    private float rotationX = 0f;
    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (playerCamera == null)
        {
            Debug.LogError("Player Camera is not assigned in the Inspector.");
        }

        LockCursor();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            LockCursor();
        }

        HandleMovement();
    }

    void LateUpdate()
    {
        HandleMouseLook();
    }

    void HandleMovement()
    {
        float verticalVelocity = moveDirection.y;

        float verticalInput = Input.GetAxisRaw("Vertical");     // W/S
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 horizontalMove = (forward * verticalInput) + (right * horizontalInput);

        if (horizontalMove.magnitude > 1f)
        {
            horizontalMove.Normalize();
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isCrouching = Input.GetKey(KeyCode.R);

        float currentSpeed;

        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
            characterController.height = crouchHeight;
        }
        else if (isRunning)
        {
            currentSpeed = runSpeed;
            characterController.height = defaultHeight;
        }
        else
        {
            currentSpeed = walkSpeed;
            characterController.height = defaultHeight;
        }

        moveDirection = horizontalMove * currentSpeed;
        moveDirection.y = verticalVelocity;

        if (Input.GetButtonDown("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }

        if (characterController.isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = -1f;
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (canMove)
        {
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }

    void HandleMouseLook()
    {
        if (!canMove || playerCamera == null)
            return;

        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        float mouseX = Input.GetAxisRaw("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxisRaw("Mouse Y") * lookSpeed;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}