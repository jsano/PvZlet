using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garlic : Nut
{

    public override float ReceiveDamage(float dmg, GameObject source, bool eat = false, bool disintegrating = false)
    {
        if (eat)
        {
            Zombie z = source.GetComponent<Zombie>();
            int newRow;
            if (z.row == ZombieSpawner.Instance.lanes) newRow = z.row - 1;
            else if (z.row == 1) newRow = z.row + 1;
            else newRow = z.row + 1 - 2 * Random.Range(0, 2);
            z.MoveToLane(newRow, 2);
        }
        return base.ReceiveDamage(dmg, source, eat, disintegrating);
    }

}
