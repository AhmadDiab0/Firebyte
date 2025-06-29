using UnityEngine;

public class HealthCollectible : MonoBehaviour
{

    [SerializeField] private float healthValue;
    [Header("SFX")]
    [SerializeField] private AudioClip pickupSound; // optional: sound effect for pickup

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Assuming the player has a Health component that manages health
            SoundManager.instance.PlaySound(pickupSound); // Play pickup sound if set
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.IncreaseHealth(healthValue);
                gameObject.SetActive(false); // Disable the collectible after pickup
            }
        }
    }
}
