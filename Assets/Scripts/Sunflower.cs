using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : Plant
{

    protected override void Attack(Zombie z)
    {
        Instantiate(projectile, transform.position + topOffset, Quaternion.identity);
    }

}
