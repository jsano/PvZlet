using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iceshroom : Plant
{

    protected override void Attack(Zombie z)
    {
        RaycastHit2D[] zombies = Physics2D.BoxCastAll(transform.position, 20 * Tile.TILE_DISTANCE, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie", "ExplosivesOnly"));
        foreach (RaycastHit2D hit in zombies)
        {
            Zombie target = hit.collider.GetComponent<Zombie>();
            if (Tile.WORLD_TO_COL(target.transform.position.x - Tile.TILE_DISTANCE.x / 3) == 10) continue;
            ((StatMod) ScriptableObject.CreateInstance("StatMod")).Apply(target, "Freeze");
            target.ReceiveDamage(damage, gameObject, disintegrating: true);
        }
        Destroy(gameObject);
    }

}
