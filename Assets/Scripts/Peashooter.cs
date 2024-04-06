using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : Plant
{

    /// <summary> Shoots a single pea </summary>
    protected override void Attack(Zombie z)
    {
        Shoot();
        base.Attack(z);
    }

    protected void Shoot()
    {
        StraightProjectile p = Instantiate(projectile, transform.position + rightOffset, projectile.transform.rotation).GetComponent<StraightProjectile>();
        p.Setup(gameObject, Vector3.right);
    }

}
