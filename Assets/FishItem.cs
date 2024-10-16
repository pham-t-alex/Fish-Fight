using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FishItem : MonoBehaviour
{
    [SerializeField] private float lifespan;
    [SerializeField] private float countdown;
    
    // Start is called before the first frame update
    void Start()
    {
        countdown = lifespan;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null)
        {
            return;
        }
        Destroy(this.gameObject);
    }
}