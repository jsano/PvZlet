using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garlic : Plant
{

    public override void ReceiveDamage(float dmg, GameObject source, bool eat = false)
    {
        base.ReceiveDamage(dmg, source, eat);
        if (eat)
        {
            Zombie z = source.GetComponent<Zombie>();
            int newRow;
            if (z.row == ZS.lanes) newRow = z.row - 1;
            else if (z.row == 1) newRow = z.row + 1;
            else newRow = z.row + 1 - 2 * Random.Range(0, 2);
            z.MoveToLane(newRow, 2);
        }
        
    }

}
