using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikeweed : Plant
{
    
    public int popsRemaining;

    public AudioClip popSFX;

    protected override void Attack(Zombie z)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(Tile.TILE_DISTANCE.x, Tile.TILE_DISTANCE.y / 2), 0, Vector2.right, 0, LayerMask.GetMask("Zombie"));
        foreach (RaycastHit2D r in hits)
        {
            if (r.collider.GetComponent<Zombie>().wheels && popsRemaining > 0)
            {
                SFX.Instance.Play(popSFX);
                r.collider.GetComponent<Zombie>().ReceiveDamage(1000, gameObject);
                popsRemaining -= 1;
            }
            else r.collider.GetComponent<Zombie>().ReceiveDamage(damage, null);
        }
        base.Attack(z);
        if (popsRemaining <= 0) Die();
    }

    public override float ReceiveDamage(float dmg, GameObject source, bool eat = false, bool disintegrating = false)
    {
        if (source != null && source.GetComponent<Zombie>() != null && (source.GetComponent<Zombie>().wheels || source.GetComponent<Gargantuar>() != null))
        {
            if (source.GetComponent<Zombie>().wheels && popsRemaining > 0)
            {
                SFX.Instance.Play(popSFX);
                source.GetComponent<Zombie>().ReceiveDamage(1000, gameObject);
            }
            popsRemaining -= 1;
            if (popsRemaining <= 0) Die();
            return 0;
        }
        return base.ReceiveDamage(dmg, source, eat, disintegrating);
    }

}
