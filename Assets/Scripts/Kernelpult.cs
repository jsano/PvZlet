using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kernelpult : Plant
{

    private int butter = 4;
    private int count;
    public GameObject butterProjectile;

    protected override void Attack(Zombie z)
    {
        LobbedProjectile p;
        count += 1;
        if (count >= butter)
        {
            p = Instantiate(butterProjectile, transform.position + topOffset, Quaternion.identity).GetComponent<LobbedProjectile>();
            count = 0;
        }
        else
        {
            p = Instantiate(projectile, transform.position + topOffset, Quaternion.identity).GetComponent<LobbedProjectile>();
        }
        p.distance = z.transform.position - p.transform.position;
        p.lane = row;
        base.Attack(z);
    }

}
