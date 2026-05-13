using UnityEngine;

/// <summary>
/// First-person-style controller:
///   • Mouse X  → rotates the player (and child camera) left/right (yaw)
///   • Mouse Y  → pitches the child camera up/down only (no body tilt)
///   • WASD     → moves relative to the player's current forward
///   • Animator → "isWalking" bool drives Idle ↔ Walking states
///
/// Setup:
///   1. Attach this script to your player GameObject.
///   2. Place the Camera as a child of the player (any position/offset you like).
///   3. Assign the child Camera to "Player Camera" in the Inspector,
///      or leave it empty — the script will find it automatically.
///   4. In the Animator add a bool parameter named "isWalking".
///      Transition  Idle → Walking  : isWalking = true
///      Transition  Walking → Idle  : isWalking = false
///      Uncheck "Has Exit Time" on both transitions.
/// </summary>
[RequireComponent(typeof(Animator))]
public class MovementController : MonoBehaviour
{
    // ── Inspector ──────────────────────────────────────────────────────────

    [Header("References")]
    [Tooltip("Child Camera. Auto-detected if left empty.")]
    [SerializeField] private Camera playerCamera;

    [Header("Mouse Look")]
    [Tooltip("Mouse sensitivity for horizontal (yaw) rotation.")]
    [SerializeField] private float mouseSensitivityX = 2f;

    [Tooltip("Mouse sensitivity for vertical (pitch) rotation.")]
    [SerializeField] private float mouseSensitivityY = 2f;

    [Tooltip("Maximum degrees the camera can look up.")]
    [SerializeField] private float pitchMin = -80f;

    [Tooltip("Maximum degrees the camera can look down.")]
    [SerializeField] private float pitchMax = 80f;

    [Header("Movement")]
    [Tooltip("Units per second.")]
    [SerializeField] private float moveSpeed = 5f;

    // ── Private state ──────────────────────────────────────────────────────

    private Animator _animator;

    // Accumulated vertical (pitch) angle — tracked separately so we can clamp it
    private float _cameraPitch = 0f;

    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");

    // ── Unity lifecycle ────────────────────────────────────────────────────

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }

        // Lock & hide the cursor for a proper FPS feel
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }

    private void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    // ── Mouse look ─────────────────────────────────────────────────────────

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;

        // ── Horizontal: rotate the whole player body on Y axis ──
        // WASD will then always move relative to this new forward.
        transform.Rotate(Vector3.up, mouseX, Space.Self);

        // ── Vertical: tilt only the child camera, never the body ──
        if (playerCamera != null)
        {
            _cameraPitch -= mouseY;                          // subtract = look up when mouse moves up
            _cameraPitch  = Mathf.Clamp(_cameraPitch, pitchMin, pitchMax);

            playerCamera.transform.localRotation =
                Quaternion.Euler(_cameraPitch, 0f, 0f);
        }
    }

    // ── WASD movement ──────────────────────────────────────────────────────

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // A / D
        float vertical   = Input.GetAxisRaw("Vertical");   // W / S

        bool isMoving = (horizontal != 0f || vertical != 0f);

        if (isMoving)
        {
            // Move relative to the player's own forward/right (already rotated by mouse)
            Vector3 moveDir = (transform.forward * vertical +
                               transform.right   * horizontal).normalized;

            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        _animator.SetBool(IsWalkingHash, isMoving);
    }
}