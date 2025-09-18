using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int maxLives = 3;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private PlayerMovement playerMovement;

    private int currentLives;
    private Vector3 lastCheckpoint;
    private Vector3 startPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentLives = maxLives;
        Debug.Log("GameManager started with " + maxLives + " lives");

        // Find UI reference if needed
        if (livesText == null)
            livesText = FindFirstObjectByType<TMPro.TextMeshProUGUI>();

        if (playerMovement == null)
            playerMovement = FindFirstObjectByType<PlayerMovement>();

        if (playerMovement != null)
        {
            startPosition = playerMovement.transform.position;
            lastCheckpoint = startPosition;
        }

        UpdateLivesUI();
    }

    public void LoseLife()
    {
        // Visual feedback first
        var playerAnimator = FindFirstObjectByType<PlayerAnimator>();
        playerAnimator?.TriggerHitAnimation();
        Debug.Log("Hit animation triggered");

        // Apply minimal knockback
        if (playerMovement != null)
        {
            // Calculate knockback direction: assume backward from current facing
            Vector2 knockbackDirection = Vector2.left; // Default left, could be improved
            playerMovement.ApplyKnockback(knockbackDirection, 1.5f);
            Debug.Log("Minimal knockback applied");
        }

        currentLives--;
        Debug.Log("Life lost, lives remaining: " + currentLives);
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            // Game Over - restart level instantly
            Debug.Log("Game Over! Restarting level...");
            RestartLevel();
        }
        else
        {
            // Still have lives - continue playing from current position
            Debug.Log("Continue playing with " + currentLives + " lives remaining");
        }
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpoint = checkpointPosition;
        Debug.Log("Checkpoint set");
    }

    private void UpdateLivesUI()
    {
        // Try to find livesText if it's null
        if (livesText == null)
        {
            livesText = FindFirstObjectByType<TMPro.TextMeshProUGUI>();
            Debug.Log("Attempting to recover livesText reference...");
        }

        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
            Debug.Log("Lives UI updated to: " + currentLives);
        }
        else
        {
            Debug.Log("ERROR: livesText is null! Cannot update UI");
        }
    }

    private void RestartLevel()
    {
        Debug.Log("Level restarted");
        currentLives = maxLives;
        Debug.Log("Lives reset to: " + currentLives);

        // Reset player position instead of reloading scene
        if (playerMovement == null)
            playerMovement = FindFirstObjectByType<PlayerMovement>();

        if (playerMovement != null)
        {
            playerMovement.transform.position = startPosition;
            playerMovement.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            Debug.Log("Player reset to start position");
        }

        UpdateLivesUI();
    }

    public void ResetLives()
    {
        currentLives = maxLives;
        UpdateLivesUI();
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (playerMovement != null)
        {
            playerMovement.transform.position = lastCheckpoint;
            playerMovement.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            Debug.Log("Player respawned at checkpoint");
        }
    }

    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RestartLevel();
    }

    public int CurrentLives => currentLives;
}