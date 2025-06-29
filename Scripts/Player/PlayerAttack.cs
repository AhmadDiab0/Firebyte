using Unity.Mathematics;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    private Animator anim;
    private PlayerMovement playerMovement;

    private float cooldownTimer = Mathf.Infinity; // Start with an infinite cooldown to allow the first attack immediately

    private void Awake()
    {
        // Reference the Animator component
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        // Check for attack input and cooldown
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack())
        {
            Attack();
            cooldownTimer = 0f;
        }
    }

    private void Attack()
    {
        // Trigger the attack animation
        anim.SetTrigger("attack");
        // Place attack logic here (e.g., damage enemies)
    }
}