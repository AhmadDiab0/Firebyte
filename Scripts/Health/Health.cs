using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOffFlashes;

    [SerializeField] private AudioClip deathSound; // optional: sound effect for death
    [SerializeField] private AudioClip hurtSound; // optional: sound effect for hurt
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        // Initialize current health to starting health
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= _damage;

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability()); // Start invulnerability frames
            SoundManager.instance.PlaySound(hurtSound); // Play hurt sound if set
        }
        else
        {
            if (!dead)
            {
                dead = true;
                // If this is an enemy, call its Die() method
                MeleeEnemy meleeEnemy = GetComponent<MeleeEnemy>();
                if (meleeEnemy != null)
                    meleeEnemy.Die();

                anim.SetTrigger("die");
                var playerMovement = GetComponent<PlayerMovement>();
                if (playerMovement != null)
                    playerMovement.enabled = false; // Disable player movement on death

                SoundManager.instance.PlaySound(deathSound); // Play death sound if set
            }
        }
    }

    public void IncreaseHealth(float _healthValue)
    {
        currentHealth = Mathf.Clamp(currentHealth + _healthValue, 0, startingHealth);
        // Optionally, you can trigger a health increase animation or effect here
    }

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true); // Ignore collisions between player and enemies

        for (int i = 0; i < numberOffFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f); // Flash red
            yield return new WaitForSeconds(iFramesDuration / (numberOffFlashes * 2));
            spriteRend.color = Color.white; // Reset color
            yield return new WaitForSeconds(iFramesDuration / (numberOffFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(8, 9, false); // Re-enable collisions after invulnerability
    }
}
