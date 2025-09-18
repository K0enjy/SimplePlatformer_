using UnityEngine;

public class GoalFlag : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool stopPlayerOnWin = true;

    private bool levelCompleted = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!levelCompleted && other.CompareTag(playerTag))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                CompleteLevel(playerMovement);
            }
        }
    }

    private void CompleteLevel(PlayerMovement player)
    {
        levelCompleted = true;

        Debug.Log("Level Complete!");

        if (stopPlayerOnWin)
        {
            player.SetMovementEnabled(false);
        }

        // Stop background music on level completion
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.StopBackgroundMusic();
        }
    }
}