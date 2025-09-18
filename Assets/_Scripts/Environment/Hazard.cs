using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Spike touched!");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoseLife();
            }
        }
    }
}