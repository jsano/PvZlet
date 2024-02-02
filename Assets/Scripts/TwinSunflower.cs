using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinSunflower : Sunflower
{

    /// <summary> Drops 2 suns </summary>
    protected override void Attack(Zombie z)
    {
        base.Attack(z);
        base.Attack(z);
    }

}
