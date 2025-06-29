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
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability()); // Start invulnerability frames
        }
        else
        {
            if (!dead)
                anim.SetTrigger("die");
            GetComponent<PlayerMovement>().enabled = false; // Disable player movement on death
            dead = true;
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
