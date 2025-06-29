using System.Collections;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float damage;
    [Header("Fire Trap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    [SerializeField] private float damageCooldown = 1f; // Damage interval in seconds

    [Header("SFX")]
    [SerializeField] private AudioClip fireTrapSound; // optional: sound effect for fire trap activation

    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered; // To check if the trap has been triggered
    private bool active; // To check if the trap is currently active
    private float damageTimer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (active)
            damageTimer += Time.deltaTime;
        else
            damageTimer = damageCooldown; // Reset so damage applies instantly next time
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!triggered)
            {
                StartCoroutine(ActivateFiretrap());
            }
            if (active && damageTimer >= damageCooldown)
            {
                collision.GetComponent<Health>()?.TakeDamage(damage);
                damageTimer = 0f;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && active && damageTimer >= damageCooldown)
        {
            collision.GetComponent<Health>()?.TakeDamage(damage);
            damageTimer = 0f;
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        // turn the sprite red to indicate activation
        triggered = true;
        spriteRend.color = Color.red;

        // wait for delay, active trap, turn animation, return color to normal
        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(fireTrapSound); // Play fire trap sound if set
        spriteRend.color = Color.white;
        active = true;
        anim.SetBool("activated", true);

        // wait for active time, then deactivate trap
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}
