using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangleKelp : Chomper
{

    protected override void Attack(Zombie z)
    {
        base.Attack(z);
        Destroy(gameObject);
    }

}
