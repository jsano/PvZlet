using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryBomb : Plant
{
    
    protected override void Attack()
    {
        GameObject g = Instantiate(projectile, transform.position, Quaternion.identity);
        g.transform.localScale = Tile.TILE_DISTANCE * 3;
        RaycastHit2D[] all = Physics2D.BoxCastAll(transform.position, Tile.TILE_DISTANCE * 3, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie"));
        foreach (RaycastHit2D a in all)
        {
            a.collider.GetComponent<Zombie>().ReceiveDamage(damage);
        }
        Destroy(gameObject);
    }

}
