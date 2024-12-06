using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FishItem : MonoBehaviour
{
    [SerializeField] private float lifespan;
    [SerializeField] private float countdown;
    [SerializeField] private Fish fish;
    
    // Start is called before the first frame update
    void Start()
    {
        countdown = lifespan;
        RandomFish();
    }

    public void RandomFish()
    {
        int value = Random.Range(0, 4);
        switch (value)
        {
            case 0:
                fish = new BasicFish();
                break;
            case 1:
                fish = new Pufferfish();
                GetComponent<SpriteRenderer>().color = new Color(243f/255, 242f/255, 170f/255);
                break;
            case 2:
                fish = new Shark();
                GetComponent<SpriteRenderer>().color = new Color(100f / 255, 123f / 255, 127f / 255);
                break;
            case 3:
                fish = new Swordfish();
                GetComponent<SpriteRenderer>().color = new Color(64f / 255, 104f / 255, 177f / 255);
                break;
        }
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
        if (player.PickupFish(fish))
        {
            Destroy(this.gameObject);
        }
    }
}