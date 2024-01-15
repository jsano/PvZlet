using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : Plant
{

    protected override void Attack()
    {
        Instantiate(projectile, transform.position + rightOffset, Quaternion.identity);
    }

}
