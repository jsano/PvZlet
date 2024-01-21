using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : Plant
{

    /// <summary> Drops sun </summary>
    protected override void Attack(Zombie z)
    {
        GameObject g = Instantiate(projectile, transform.position + topOffset, Quaternion.identity);
        g.GetComponent<Sun>().ground = transform.position.y;
    }

}
