using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class PlayerHealth : MonoBehaviour
{
    private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LinkToPlayer(Player p)
    {
        p.HealthChangeEvent += UpdateHealth;
        UpdateHealth(p.Health);
    }

    public void UpdateHealth(int health)
    {
        text.text = health + "";
    }
}
