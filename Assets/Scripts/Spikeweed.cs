using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikeweed : Plant
{

    protected override void Attack(Zombie z)
    {
        bool popped = false;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(Tile.TILE_DISTANCE.x, Tile.TILE_DISTANCE.y / 2), 0, Vector2.right, 0, LayerMask.GetMask("Zombie"));
        foreach (RaycastHit2D r in hits)
        {
            if (r.collider.GetComponent<Zombie>().wheels)
            {
                r.collider.GetComponent<Zombie>().ReceiveDamage(1000, gameObject);
                popped = true;
            }
            else r.collider.GetComponent<Zombie>().ReceiveDamage(damage, null);
        }
        base.Attack(z);
        if (popped) Die();
    }

    public override void ReceiveDamage(float dmg, GameObject source, bool eat = false)
    {
        if (source != null && source.GetComponent<Zombie>() != null && source.GetComponent<Zombie>().wheels)
        {
            source.GetComponent<Zombie>().ReceiveDamage(1000, gameObject);
            Die();
        }
        else base.ReceiveDamage(dmg, source, eat);
    }

}
