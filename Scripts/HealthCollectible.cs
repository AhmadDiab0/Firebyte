using UnityEngine;

public class HealthCollectible : MonoBehaviour
{

    [SerializeField] private float healthValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Assuming the player has a Health component that manages health
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.IncreaseHealth(healthValue);
                gameObject.SetActive(false); // Disable the collectible after pickup
            }
        }
    }
}
