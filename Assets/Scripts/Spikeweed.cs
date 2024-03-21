using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikeweed : Plant
{
    
    public int popsRemaining;

    protected override void Attack(Zombie z)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(Tile.TILE_DISTANCE.x, Tile.TILE_DISTANCE.y / 2), 0, Vector2.right, 0, LayerMask.GetMask("Zombie"));
        foreach (RaycastHit2D r in hits)
        {
            if (r.collider.GetComponent<Zombie>().wheels && popsRemaining > 0)
            {
                r.collider.GetComponent<Zombie>().ReceiveDamage(1000, gameObject);
                popsRemaining -= 1;
            }
            else r.collider.GetComponent<Zombie>().ReceiveDamage(damage, null);
        }
        base.Attack(z);
        if (popsRemaining <= 0) Die();
    }

    public override void ReceiveDamage(float dmg, GameObject source, bool eat = false)
    {
        if (source != null && source.GetComponent<Zombie>() != null && (source.GetComponent<Zombie>().wheels || source.GetComponent<Gargantuar>() != null))
        {
            if (source.GetComponent<Zombie>().wheels && popsRemaining > 0) source.GetComponent<Zombie>().ReceiveDamage(1000, gameObject);
            popsRemaining -= 1;
            if (popsRemaining <= 0) Die();
        }
        else base.ReceiveDamage(dmg, source, eat);
    }

}
