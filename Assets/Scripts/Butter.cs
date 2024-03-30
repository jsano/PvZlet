using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butter : LobbedProjectile
{

    /// <summary> Applies the butter effect to the hit zombie, and then continues with the default behavior </summary>
    protected override void Hit(Damagable other, float amount)
    {
        Zombie z = other.GetComponent<Zombie>();
        if (z != null) ((StatMod)ScriptableObject.CreateInstance("StatMod")).Apply(z, "Butter");
        base.Hit(other, amount);
    }

}
