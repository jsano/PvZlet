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
            if (target.GetComponent<Collider2D>().enabled && target.GetComponent<Balloon>() == null
                && target.gameObject.layer == LayerMask.NameToLayer("Zombie"))
            {
                ((StatMod) ScriptableObject.CreateInstance("StatMod")).Apply(target, "Freeze");
                target.ReceiveDamage(damage, null, disintegrating: true);
            }
        }
        Destroy(gameObject);
    }

}
