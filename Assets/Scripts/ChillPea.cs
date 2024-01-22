using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillPea : StraightProjectile
{

    /// <summary> Applies the chill effect to the hit zombie, and then continues with the default behavior </summary>
    protected override void Hit(GameObject other)
    {
        Zombie z = other.GetComponent<Zombie>();
        if (z != null) ((StatMod) ScriptableObject.CreateInstance("StatMod")).Apply(z, "chill");
        base.Hit(other);
    }

}
