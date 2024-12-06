using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFish : Fish
{
    public BasicFish()
    {
        SetUses(3);
    }

    public override void Use()
    {
        GameObject attackRange = GameObject.Instantiate(PrefabManager.Instance.BasicFishAttackPrefab);

        if (attackRange.TryGetComponent(out AttackArea attackArea))
        {
            attackArea.setDirectionFacing(player.MovedRightLast);
            attackArea.ThisObjectCreator(player);
        }

        if (player.MovedRightLast)
        { // moved right last
          //GameObject attackRange = Instantiate(attackObject);
            attackRange.transform.position = new Vector2((player.transform.position.x + 1.5f), player.transform.position.y);
            Debug.Log("Instantiated attack to the right!");
        }
        else
        { // moved left last
          //GameObject attackRange = Instantiate(attackObject);
            attackRange.transform.position = new Vector2((player.transform.position.x - 1.5f), player.transform.position.y);
            Debug.Log("Instantiated attack to the left!");
        }
        GameObject.Destroy(attackRange, 0.1f /* This number is how long the attack will last*/);
    }
}