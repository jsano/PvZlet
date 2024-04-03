using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : Zombie
{

    // Update is called once per frame
    public override void Update()
    {
        if (shield == null) walkTime = alternateWalkTime[0];
        base.Update();
    }

    protected override void Eat(GameObject p)
    {
        if (shield != null)
        {
            Destroy(shield);
            if (p.GetComponent<Zombie>() != null)
            {
                p.GetComponent<Damagable>().ReceiveDamage(100, null, disintegrating: true);
            }
            else
            {
                Plant p1 = p.GetComponent<Plant>();
                Tile.tileObjects[p1.row, p1.col].RemoveAllPlants();
            }
            ResetWalk();
        }
        else base.Eat(p);
    }

}
