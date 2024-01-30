using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabbagepult : Plant
{

    protected override void Attack(Zombie z)
    {
        LobbedProjectile p = Instantiate(projectile, transform.position + topOffset, Quaternion.identity).GetComponent<LobbedProjectile>();
        p.distance = z.transform.position - p.transform.position;
        p.lane = row;
    }

}
