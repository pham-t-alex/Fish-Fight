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
    [SerializeField] private double damage = 3.0;
    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Attack object created, has triggered");
        if (collider.GetComponent<Player>() != null) {
            Debug.Log("Have detected a player");
            Player p = collider.GetComponent<Player>();
            p.Hurt(damage);
        }
    }
}
