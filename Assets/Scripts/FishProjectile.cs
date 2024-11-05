using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishProjectile : MonoBehaviour
{
    private Player attacker;
    [SerializeField] private int damage = 1;
    [SerializeField] private float stunDuration = 1.0f;
    [SerializeField] private float xDirectionKnockback = 500.0f;
    [SerializeField] private float yDirectionKnockback = 1.0f;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Player p)
    {
        attacker = p;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        Player p = collision.GetComponent<Player>();
        if (tag == "Ground" || tag == "Platform" || tag == "Border")
        {
            Destroy(gameObject);
        }
        else if (p != null && p != attacker)
        {
            Vector2 vel = GetComponent<Rigidbody2D>().velocity;
            if (vel.x < 0)
            {
                xDirectionKnockback *= -1;
            }
            else if (vel.x == 0)
            {
                xDirectionKnockback = 0;
            }
            p.Hurt(damage, new Vector2(xDirectionKnockback, yDirectionKnockback));
            p.Stun(stunDuration);
            Destroy(gameObject);
        }
    }
}
