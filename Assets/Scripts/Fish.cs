using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fish
{
    private int maxUses;
    private float maxTime;
    protected Player player;

    protected void SetUsesAndTime(int uses, float time)
    {
        maxUses = uses;
        maxTime = time;
    }

    public abstract void Use();

    public int GetMaxUses()
    {
        return maxUses;
    }

    public float GetMaxTime()
    {
        return maxTime;
    }

    public void SetPlayer(Player p)
    {
        player = p;
    }
}