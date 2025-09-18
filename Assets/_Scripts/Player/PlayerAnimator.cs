using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (playerMovement == null || animator == null) return;

        bool isGrounded = playerMovement.IsGrounded;
        float horizontalInput = playerMovement.HorizontalInput;

        // Calculate speed first
        float speed = isGrounded ? Mathf.Abs(playerMovement.Velocity.x) : 0f;

        // Crouch logic: only when grounded, Ctrl pressed, AND not moving
        bool isCrouching = Keyboard.current.leftCtrlKey.isPressed && isGrounded && speed <= 0.1f;

        // Update animator parameters
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsCrouching", isCrouching);

        // Handle sprite flipping based on horizontal input
        if (spriteRenderer != null && horizontalInput != 0f)
        {
            spriteRenderer.flipX = horizontalInput < 0f;
        }
    }

    public void TriggerHitAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("IsHit");
        }
    }
}