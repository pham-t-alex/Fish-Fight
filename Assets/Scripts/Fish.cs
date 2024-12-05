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

    public virtual void Throw()
    {
        GameObject fishProj = GameObject.Instantiate(PrefabManager.Instance.FishProjPrefab,
            player.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        fishProj.GetComponent<FishProjectile>().Initialize(player);
        if (player.MovedRightLast)
        {
            fishProj.GetComponent<Rigidbody2D>().AddForce(new Vector2(25, 2), ForceMode2D.Impulse);
        }
        else
        {
            fishProj.GetComponent<Rigidbody2D>().AddForce(new Vector2(-25, 2), ForceMode2D.Impulse);
        }
    }

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