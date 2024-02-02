using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fumeshroom : Plant
{

    protected override void Attack(Zombie z)
    {
        GameObject g = Instantiate(projectile, transform.position + rightOffset, Quaternion.identity);
        g.transform.localScale = new Vector3(Tile.TILE_DISTANCE.x * range, 1, 1);
        g.transform.position += new Vector3(Tile.TILE_DISTANCE.x * range / 2, 0, 0);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, (range + 0.5f) * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie", "Shield"));
        foreach (RaycastHit2D r in hits)
        {
            r.collider.GetComponent<Damagable>().ReceiveDamage(damage, null);
        }
        base.Attack(z);
    }

}
