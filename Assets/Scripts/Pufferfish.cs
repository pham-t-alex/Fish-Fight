using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pufferfish : Fish
{
    public Pufferfish()
    {
        SetUsesAndTime(1, 5);
    }

    public override void Use()
    {
        GameObject fishProj = GameObject.Instantiate(PrefabManager.Instance.PufferProjPrefab,
            player.transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);
        fishProj.GetComponent<PufferfishProjectile>().Initialize(player);
        if (player.MovedRightLast)
        {
            fishProj.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000, -1000));
        }
        else
        {
            fishProj.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1000, -1000));
        }

        /*GameObject attackRange = GameObject.Instantiate(FishItemSpawnManager.Instance.FishPrefab(0));

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
        GameObject.Destroy(attackRange, 0.5f);*/
    }
}
