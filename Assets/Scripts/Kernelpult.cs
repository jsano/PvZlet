using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kernelpult : Cabbagepult
{

    private int butter = 3;
    private int count;
    public GameObject butterProjectile;

    protected override void Attack(Zombie z)
    {
        count += 1;
        if (count >= butter)
        {
            LobbedProjectile p = Instantiate(butterProjectile, transform.position + topOffset, Quaternion.identity).GetComponent<LobbedProjectile>();
            p.distance = z.transform.position - p.transform.position;
            p.lane = row;
            count = 0;
        }
        else base.Attack(z);
    }

}
