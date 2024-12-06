using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialAttackArea : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float stunDuration = 0.4f;
    //[SerializeField] private float knockback = 1.0f;
    [SerializeField] private float knockback = 5;
    private Player thisPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThisObjectCreator(Player p)
    {
        thisPlayer = p;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.GetComponent<Player>();
        if (p != null && p != thisPlayer)
        {
            Vector2 kb = (p.transform.position - transform.position).normalized * knockback;
            p.Hurt(damage, kb);
            p.Stun(stunDuration);
        }
    }
}
