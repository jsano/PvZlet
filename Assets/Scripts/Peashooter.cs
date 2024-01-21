using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : Plant
{

    /// <summary> Shoots a single pea </summary>
    protected override void Attack(Zombie z)
    {
        Instantiate(projectile, transform.position + rightOffset, Quaternion.identity);
    }

}
