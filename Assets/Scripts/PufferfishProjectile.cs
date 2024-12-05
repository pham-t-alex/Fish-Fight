using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferfishProjectile : FishProjectile
{
    private enum State
    {
        Thrown,
        Bounced,
        Inflated,
        Slamming
    }
    private State state = State.Thrown;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (state == State.Bounced)
        {
            int mask = 1 << 3;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, mask);
            if (hit && hit.collider.GetComponent<Player>() != attacker && hit.transform.position.y < transform.position.y - 3)
            {
                state = State.Inflated;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
                transform.localScale = new Vector2(transform.localScale.x * 3, transform.localScale.y * 3);
                StartCoroutine(TriggerSlam());
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        Player p = collision.GetComponent<Player>();
        if (p != null && p == attacker)
        {
            return;
        }
        if (state == State.Slamming)
        {
            if (p != null)
            {
                Damage(p);
                Destroy(gameObject);
            }
        }
        else if (state == State.Bounced)
        {
            if (p != null || tag == "Ground" || tag == "Border")
            {
                Destroy(gameObject);
            }
        }
        else if (state == State.Thrown)
        {
            if (tag == "Ground" || tag == "Platform")
            {
                state = State.Bounced;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(rb.velocity.y));
            }
            else if (p != null || tag == "Border")
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator TriggerSlam()
    {
        yield return new WaitForSeconds(0.2f);
        state = State.Slamming;
        rb.AddForce(new Vector2(0, -1000), ForceMode2D.Impulse);
        Destroy(gameObject, 5);
    }
}
