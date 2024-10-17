using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [SerializeField] private int damage = 1;
    [SerializeField] private float stunDuration = 1.0f;
    [SerializeField] private float knockback = 1.0f;
    private bool facingRight = true;
    //private Player p = null;
    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Attack object created, has triggered");
        if (collider.GetComponent<Player>() != null) {
            Debug.Log("Have detected a player");
            Player p = collider.GetComponent<Player>();
            if (!facingRight) knockback *= -1;
            Debug.Log("knockback: " + knockback);
            p.Hurt(damage, knockback);
            p.Stun(stunDuration);
        }
    }
    public void setDirectionFacing(bool direction) {
        facingRight = direction;
    }
}
