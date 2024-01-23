using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iceshroom : Plant
{

    protected override void Attack(Zombie z)
    {
        Zombie[] zombies = FindObjectsByType<Zombie>(FindObjectsSortMode.None);
        foreach (Zombie target in zombies)
        {
            if (target.gameObject.layer == LayerMask.NameToLayer("Zombie"))
            {
                ((StatMod) ScriptableObject.CreateInstance("StatMod")).Apply(target, "freeze");
                target.ReceiveDamage(damage, null);
            }
        }
        Destroy(gameObject);
    }

}
