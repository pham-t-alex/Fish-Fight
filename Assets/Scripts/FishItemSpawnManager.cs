using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishItemSpawnManager : MonoBehaviour
{
    private List<(FishItemSpawn, float)> spawnPlatforms = new List<(FishItemSpawn, float)>();
    private float totalWeight = 0;
    [Header("Spawn Timing")]
    [SerializeField] private float initialSpawnDelay = 1;
    [SerializeField] private float spawnDelay = 5;
    [SerializeField] private float currentDelay;
    [Header("Spawn Details")]
    [SerializeField] private GameObject fishItem;
    [SerializeField] private int fishCount;
    private static FishItemSpawnManager instance;
    [Header("Fish Prefabs")]
    [SerializeField] private List<GameObject> fishPrefabs = new List<GameObject>();
    public GameObject FishPrefab(int index)
    {
        return fishPrefabs[index];
    }

    public static FishItemSpawnManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FishItemSpawnManager>();
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            currentDelay = initialSpawnDelay;
        }
    }

    public void AddSpawnPlatform(FishItemSpawn spawn, float weight)
    {
        spawnPlatforms.Add((spawn, weight));
        totalWeight += weight;
    }

    // Update is called once per frame
    void Update()
    {
        currentDelay -= Time.deltaTime;
        if (currentDelay <= 0)
        {
            SpawnFish();
            currentDelay = spawnDelay;
        }
    }

    private void SpawnFish()
    {
        for (int i = 0; i < fishCount; i++)
        {
            float randomValue = Random.Range(0, totalWeight);
            int index = 0;
            while (index < spawnPlatforms.Count)
            {
                randomValue -= spawnPlatforms[index].Item2;
                if (randomValue <= 0)
                {
                    spawnPlatforms[index].Item1.SpawnFishItem(fishItem);
                    break;
                }
                index++;
            }
            
        }
    }
}
