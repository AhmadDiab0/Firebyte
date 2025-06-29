using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound; // Sound to play when reaching a new checkpoint
    private Transform currentCheckpoint; // The current checkpoint the player is at
    private Health playerHealth; // Reference to the player's health component

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
    }

    public void Respawn()
    {
        transform.position = currentCheckpoint.position; // Move player to the current checkpoint position
        playerHealth.Respawn(); // Reset player's health

        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform; // Update the current checkpoint
            SoundManager.instance.PlaySound(checkpointSound); // Play checkpoint sound
            collision.GetComponent<Collider2D>().enabled = false; // Disable the checkpoint collider to prevent re-triggering
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}
