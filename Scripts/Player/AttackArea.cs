using UnityEngine;

public class AttackArea : MonoBehaviour
{    private bool hit;
    private Animator anim;
    private PolygonCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        if (hit) return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
    }

    public void ResetAttackArea()
    {
        hit = false;
        boxCollider.enabled = true;
        anim.SetTrigger("reset");
    }
}
