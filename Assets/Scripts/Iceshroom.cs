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
            if (Tile.WORLD_TO_COL(target.transform.position.x - Tile.TILE_DISTANCE.x / 3) == 10) continue;
            if (target.GetComponent<Collider2D>().enabled && target.gameObject.layer == LayerMask.NameToLayer("Zombie"))
            {
                ((StatMod) ScriptableObject.CreateInstance("StatMod")).Apply(target, "Freeze");
                target.ReceiveDamage(damage, null, disintegrating: true);
            }
        }
        Destroy(gameObject);
    }

}
