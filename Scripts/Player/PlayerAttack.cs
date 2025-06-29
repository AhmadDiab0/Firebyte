using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 0.5f;  // seconds between swings
    [SerializeField] private int attackDamage = 25;
    [SerializeField] private Transform attackPoint;       // assign your child in Inspector
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private LayerMask enemyLayers;       // set to “Enemy”
    [SerializeField] private AudioClip attackSound; // optional: sound effect for attack

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack())
        {
            Attack();
        }
    }

    private void Attack()
    {
        SoundManager.instance.PlaySound(attackSound); // Play attack sound if set
        anim.SetTrigger("attack");
        cooldownTimer = 0f; // <-- Reset cooldown here

        // optional: delay the hit detection via coroutine or animation event
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position, attackRadius, enemyLayers
        );
        foreach (Collider2D hit in hits)
        {
            var health = hit.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(attackDamage);
        }
    }

    // Draw the hit area in the Scene view
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
