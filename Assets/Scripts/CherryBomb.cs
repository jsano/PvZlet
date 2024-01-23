using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryBomb : Plant
{

    public Vector2 area;

    /// <summary> Explodes in a 3x3 area, and then disappears </summary>
    protected override void Attack(Zombie z)
    {
        GameObject g = Instantiate(projectile, transform.position, Quaternion.identity);
        g.transform.localScale = area * Tile.TILE_DISTANCE;
        RaycastHit2D[] all = Physics2D.BoxCastAll(transform.position, area * Tile.TILE_DISTANCE, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie"));
        foreach (RaycastHit2D a in all)
        {
            a.collider.GetComponent<Zombie>().ReceiveDamage(damage, null);
        }
        Destroy(gameObject);
    }

}
