using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Fish
{
    public Shark()
    {
        SetUses(4);
    }

    public override void Use()
    {
        float dashTime = 0.1f;
        foreach (Player p in Player.AllPlayers)
        {
            if (p != player)
            {
                player.StartCoroutine(player.DisableCollision(p.GetComponent<Collider2D>(), dashTime + 0.1f));
            }
        }
        player.InvokeDash(dashTime, 25, 3);
        SharkAttack attack = GameObject.Instantiate(PrefabManager.Instance.SharkPrefab, player.transform).GetComponent<SharkAttack>();
        GameObject.Destroy(attack.gameObject, dashTime);
    }
}
