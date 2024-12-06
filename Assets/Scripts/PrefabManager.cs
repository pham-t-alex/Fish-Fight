using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    [SerializeField] private GameObject basicFishAttackPrefab;
    public GameObject BasicFishAttackPrefab
    {
        get
        {
            return basicFishAttackPrefab;
        }
    }
    [SerializeField] private GameObject fishProjPrefab;
    public GameObject FishProjPrefab
    {
        get
        {
            return fishProjPrefab;
        }
    }
    [SerializeField] private GameObject pufferProjPrefab;
    public GameObject PufferProjPrefab
    {
        get
        {
            return pufferProjPrefab;
        }
    }
    [SerializeField] private GameObject sharkPrefab;
    public GameObject SharkPrefab
    {
        get
        {
            return sharkPrefab;
        }
    }
    [SerializeField] private GameObject swordfishPrefab;
    public GameObject SwordfishPrefab
    {
        get
        {
            return swordfishPrefab;
        }
    }

    private static PrefabManager instance;
    public static PrefabManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PrefabManager>();
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
