using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikeweed : Plant
{

    protected override void Attack(Zombie z)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(Tile.TILE_DISTANCE.x, Tile.TILE_DISTANCE.y / 2), 0, Vector2.right, 0, LayerMask.GetMask("Zombie"));
        foreach (RaycastHit2D r in hits)
        {
            r.collider.GetComponent<Zombie>().ReceiveDamage(damage, null);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Zombie>().wheels)
        {
            HP -= 1;
            collision.GetComponent<Zombie>().ReceiveDamage(1000, gameObject);
        }
    }

}
