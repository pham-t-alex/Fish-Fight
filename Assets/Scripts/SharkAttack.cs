using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAttack : MonoBehaviour
{
    private Player attacker;
    private bool rightward;
    private HashSet<Player> hitPlayers = new HashSet<Player>();
    [SerializeField] private int damage = 3;
    [SerializeField] private float stunDuration = 0.5f;
    [SerializeField] private float xDirectionKnockback = 5f;
    [SerializeField] private float yDirectionKnockback = 3f;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Initialize(Player p)
    {
        attacker = p;
        rightward = attacker.MovedRightLast;
        GetComponent<Collider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.GetComponent<Player>();
        if (p != null && p != attacker && !hitPlayers.Contains(p))
        {
            hitPlayers.Add(p);
            if (rightward)
            {
                p.Hurt(damage, new Vector2(xDirectionKnockback, yDirectionKnockback));
            }
            else
            {
                p.Hurt(damage, new Vector2(-1 * xDirectionKnockback, yDirectionKnockback));
            }
            p.Stun(stunDuration);
        }
    }
}
