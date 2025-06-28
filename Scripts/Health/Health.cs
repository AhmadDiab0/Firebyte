using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    private void Awake()
    {
        // Initialize current health to starting health
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
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
}
