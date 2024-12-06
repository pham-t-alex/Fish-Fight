using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordfish : Fish
{
    public Swordfish()
    {
        SetUses(3);
    }

    public override void Use()
    {
        GameObject attackRange = GameObject.Instantiate(PrefabManager.Instance.SwordfishPrefab);

        if (attackRange.TryGetComponent(out RadialAttackArea attackArea))
        {
            attackArea.ThisObjectCreator(player);
        }

        attackRange.transform.position = player.transform.position;
        GameObject.Destroy(attackRange, 0.1f /* This number is how long the attack will last*/);
    }
}
