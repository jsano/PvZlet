using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryBomb : Plant
{

    private Vector2 area = Tile.TILE_DISTANCE * 2.5f;

    protected override void Attack(Zombie z)
    {
        GameObject g = Instantiate(projectile, transform.position, Quaternion.identity);
        g.transform.localScale = area;
        RaycastHit2D[] all = Physics2D.BoxCastAll(transform.position, area, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie"));
        foreach (RaycastHit2D a in all)
        {
            a.collider.GetComponent<Zombie>().ReceiveDamage(damage);
        }
        Destroy(gameObject);
    }

}
