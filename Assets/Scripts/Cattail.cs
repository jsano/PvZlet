using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cattail : Plant
{

    protected override Zombie LookInRange(int row)
    {
        Vector3 origin = new Vector3(Tile.COL_TO_WORLD[1] - Tile.TILE_DISTANCE.x, Tile.tileObjects[1, 3].transform.position.y, 0);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, new Vector2(0.1f, ZS.lanes * Tile.TILE_DISTANCE.y * 2), 0, Vector2.right, Tile.TILE_DISTANCE.x * 11, LayerMask.GetMask("Zombie"));
        foreach (RaycastHit2D hit in hits) if (hit.collider.offset.y >= 0) return hit.collider.GetComponent<Zombie>();
        return null;
    }

    protected override void Attack(Zombie z)
    {
        StartCoroutine(Attack_Helper(z));
    }

    private IEnumerator Attack_Helper(Zombie z)
    {
        HomingProjectile p = Instantiate(projectile, transform.position + topOffset, projectile.transform.rotation).GetComponent<HomingProjectile>();
        p.toChase = (z == null) ? null : z.gameObject;
        yield return new WaitForSeconds(0.5f);
        p = Instantiate(projectile, transform.position + topOffset, projectile.transform.rotation).GetComponent<HomingProjectile>();
        p.toChase = (z == null) ? null : z.gameObject;
        base.Attack(z);
    }

}
