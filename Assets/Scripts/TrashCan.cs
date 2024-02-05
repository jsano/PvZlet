using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.tvOS;

public class TrashCan : Zombie
{

    public float noTrashWalkTime;

    // Update is called once per frame
    public override void Update()
    {
        if (shield == null) walkTime = noTrashWalkTime;
        base.Update();
    }

    protected override IEnumerator Eat(Damagable p)
    {
        if (shield != null)
        {
            Destroy(shield);
            Plant p1 = p.GetComponent<Plant>();
            Tile.tileObjects[p1.row, p1.col].RemoveAllPlants();
            ResetWalk();
            yield break;
        }
        while (true) yield return base.Eat(p);
    }

}
