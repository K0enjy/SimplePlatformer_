using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayerMask = -1;
    [SerializeField] private Vector3 spawnPoint;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float horizontalInput;
    private bool jumpRequested;
    private bool movementEnabled = true;
    private bool knockbackActive = false;
    private float knockbackTimer = 0f;
    private float knockbackDuration = 0.15f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPoint = transform.position;
    }

    void Update()
    {
        // Ground check (always update)
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayerMask);

        // Handle knockback timer
        if (knockbackActive)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                knockbackActive = false;
            }
            return; // Skip input during knockback
        }

        if (!movementEnabled) return;

        // Read horizontal input using Input System
        horizontalInput = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            horizontalInput = -1f;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            horizontalInput = 1f;

        // Jump input
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        // Skip normal movement during knockback
        if (knockbackActive) return;

        if (!movementEnabled)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        // Apply horizontal movement while preserving Y velocity
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // Execute jump if requested
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }

    public void Respawn()
    {
        transform.position = spawnPoint;
        rb.linearVelocity = Vector2.zero;
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }

    public void SetMovementEnabled(bool enabled)
    {
        movementEnabled = enabled;
    }

    public void ApplyKnockback(Vector2 knockbackDirection, float force)
    {
        if (rb == null) return;

        // Apply balanced knockback: horizontal emphasis, minimal vertical
        Vector2 knockback = new Vector2(knockbackDirection.x * force, 0.2f);
        rb.linearVelocity = knockback;

        // Activate knockback state
        knockbackActive = true;
        knockbackTimer = knockbackDuration;

        // Reset input
        horizontalInput = 0f;
        jumpRequested = false;
    }

    public bool IsGrounded => isGrounded;
    public Vector2 Velocity => rb.linearVelocity;
    public float HorizontalInput => horizontalInput;
}