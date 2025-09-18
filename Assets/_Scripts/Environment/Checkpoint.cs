using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private Color activatedColor = Color.green;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isActivated = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag(playerTag))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.SetSpawnPoint(transform.position);

                // Update GameManager checkpoint
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.SetCheckpoint(transform.position);
                }

                ActivateCheckpoint();
            }
        }
    }

    private void ActivateCheckpoint()
    {
        isActivated = true;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = activatedColor;
        }

        Debug.Log("Checkpoint activated!");
    }
}