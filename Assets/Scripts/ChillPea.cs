using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillPea : StraightProjectile
{

    public AudioClip sparkle;
    public AudioClip effectSFX;

    public override void Start()
    {
        SFX.Instance.Play(sparkle);
        base.Start();
    }

    /// <summary> Applies the chill effect to the hit zombie, and then continues with the default behavior </summary>
    protected override void Hit(Damagable other)
    {
        Zombie z = other.GetComponent<Zombie>();
        if (z != null) ((StatMod) ScriptableObject.CreateInstance("StatMod")).Apply(z, "Chill", effectSFX);
        base.Hit(other);
    }

}
