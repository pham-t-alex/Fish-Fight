using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishProjectile : MonoBehaviour
{
    protected Player attacker;
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
        GetComponent<Collider2D>().enabled = true;
    }

    public void UpdateStats(int dmg, float stun, float x, float y)
    {
        damage = dmg;
        stunDuration = stun;
        xDirectionKnockback = x;
        yDirectionKnockback = y;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        Player p = collision.GetComponent<Player>();
        if (tag == "Ground" || tag == "Platform" || tag == "Border")
        {
            Destroy(gameObject);
        }
        else if (p != null && p != attacker)
        {
            Damage(p);
            Destroy(gameObject);
        }
    }

    protected void Damage(Player p)
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
    }
}
