using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fumeshroom : Plant
{

    public AudioClip[] hitSFX;

    protected override void Attack(Zombie z)
    {
        float actualRange = range;

        SFX.Instance.Play(hitSFX[Random.Range(0, hitSFX.Length)]);
        GameObject g = Instantiate(projectile, transform.position + rightOffset, Quaternion.identity);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(Tile.TILE_DISTANCE.x, 0), Vector2.right, Tile.TILE_DISTANCE.x, LayerMask.GetMask("Slope"));
        if (hit) actualRange = 1;

        g.transform.localScale = new Vector3(Tile.TILE_DISTANCE.x * actualRange, 1, 1);
        g.transform.position += new Vector3(Tile.TILE_DISTANCE.x * actualRange / 2, 0, 0);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, (actualRange + 0.5f) * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie", "Shield"));
        foreach (RaycastHit2D r in hits)
        {
            r.collider.GetComponent<Damagable>().ReceiveDamage(damage, null, disintegrating: true);
        }
        base.Attack(z);
    }

}
