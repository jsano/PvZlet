using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillPea : StraightProjectile
{
    
    protected override void Hit(Zombie other)
    {
        ((StatMod) ScriptableObject.CreateInstance("StatMod")).Apply(other, "chill");
        base.Hit(other);
    }

}
