using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFish : Fish
{
    public BasicFish()
    {
        SetUsesAndTime(3, 5);
    }

    public override void Use()
    {
        GameObject attackRange = GameObject.Instantiate(FishItemSpawnManager.Instance.FishPrefab(0));

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
        GameObject.Destroy(attackRange, 0.5f /* This number is how long the attack will last*/);
    }
}