using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillPea : StraightProjectile
{
    
    protected override void Hit(Zombie other)
    {
        if (other.status) other.status.Remove();
        other.status = (StatMod) ScriptableObject.CreateInstance("StatMod");
        other.status.Apply(other, "chill");
        base.Hit(other);
    }

}
