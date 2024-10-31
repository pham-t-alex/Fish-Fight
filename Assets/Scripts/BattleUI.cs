using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth p1HP;
    [SerializeField] private PlayerHealth p2HP;

    private int playerCount = 0;

    private static BattleUI instance;

    public static BattleUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BattleUI>();
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

    public void AddPlayer(Player p)
    {
        if (playerCount == 0)
        {
            p1HP.LinkToPlayer(p);
        }
        else if (playerCount == 1)
        {
            p2HP.LinkToPlayer(p);
        }
        playerCount++;
    }
}
