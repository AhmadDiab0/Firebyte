using UnityEngine;

public class spikehead : EnemyDamage
{
    [Header("Spikehead Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;
    private float checkTimer;
    private Vector3 destination;
    private Vector3[] directions = new Vector3[4];
    private bool attacking;

    [Header("SFX")]
    [SerializeField] private AudioClip impactSound; // optional: sound effect for impact sound
    private void OnEnable()
    {
        Stop();
    }

    private void Update()
    {
        if (attacking)
            transform.Translate(destination * Time.deltaTime * speed);
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
            {
                CheckForPlayer();
            }
        }
    }

    private void CheckForPlayer()
    {
        CalculateDirections();

        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null && !attacking)
            {
                attacking = true;
                destination = directions[i];
                checkTimer = 0; // Reset the timer after detecting a player
            }
        }
    }

    private void CalculateDirections()
    {
        directions[0] = transform.right * range; // Right
        directions[1] = -transform.right * range; // Left
        directions[2] = transform.up * range; // Up
        directions[3] = -transform.up * range; // Down
    }

    private void Stop()
    {
        destination = transform.position; // Stop moving
        attacking = false; // Reset attacking state
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(impactSound); // Play impact sound if set
        base.OnTriggerEnter2D(collision); // Call the base class method to handle damage
        Stop(); // Stop the spikehead when it collides with the player
    }
}
