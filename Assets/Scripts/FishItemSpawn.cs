using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Assumes that fish spawn surface based on collider is flat
[RequireComponent(typeof(Collider2D))]
public class FishItemSpawn : MonoBehaviour
{
    private float relY;
    private float relXBoundL;
    private float relXBoundR;
    // Start is called before the first frame update
    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        relXBoundL = -1 * collider.bounds.extents.x;
        relXBoundR = collider.bounds.extents.x;
        relY = collider.bounds.extents.y;
        FishItemSpawnManager.Instance.AddSpawnPlatform(this, relXBoundR - relXBoundL);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnFishItem(GameObject fishItem)
    {
        float x = Random.Range(transform.position.x + relXBoundL, transform.position.x + relXBoundR);
        GameObject obj = Instantiate(fishItem, new Vector2(x, transform.position.y + relY), Quaternion.identity);
    }
}
