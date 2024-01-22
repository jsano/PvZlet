using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : Plant
{

    /// <summary> Shoots a single pea </summary>
    protected override void Attack(Zombie z)
    {
        StraightProjectile p = Instantiate(projectile, transform.position + rightOffset, Quaternion.identity).GetComponent<StraightProjectile>();
        if (p.distance != range) p.distance = range;
    }

}
