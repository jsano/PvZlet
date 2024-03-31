using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryBomb : Plant
{

    public Vector2 area;

    public AudioClip buildup;

    public override void Start()
    {
        base.Start();
        if (!sleeping) SFX.Instance.Play(buildup);
    }

    /// <summary> Explodes in a 3x3 area, and then disappears </summary>
    protected override void Attack(Zombie z)
    {
        GameObject g = Instantiate(projectile, transform.position, Quaternion.identity);
        g.transform.localScale = area * Tile.TILE_DISTANCE;
        RaycastHit2D[] all = Physics2D.BoxCastAll(transform.position, area * Tile.TILE_DISTANCE, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie", "ExplosivesOnly"));
        foreach (RaycastHit2D a in all)
        {
            // Remove active ladders
            if (a.collider.GetComponent<Shield>() != null) Destroy(a.collider.gameObject); 
            else a.collider.GetComponent<Zombie>().ReceiveDamage(damage, null, disintegrating: true);
        }
        Destroy(gameObject);
    }

}
