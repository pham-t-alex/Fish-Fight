using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class PlayerHealth : MonoBehaviour
{
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
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
        slider.value = health;
    }
}
