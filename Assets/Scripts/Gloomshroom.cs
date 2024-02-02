using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloomshroom : Plant
{

    public override void Start()
    {
        base.Start();
        rightOffset = new Vector3(Tile.TILE_DISTANCE.x, 0, 0);
        topOffset = new Vector3(0, Tile.TILE_DISTANCE.y, 0);
    }

    protected override Zombie LookInRange(int row)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, Tile.TILE_DISTANCE * (range + 0.5f) * 2, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie", "Shield"));
        foreach (RaycastHit2D hit in hits) if (hit.collider.offset.y == 0) return hit.collider.GetComponent<Zombie>();
        return null;
    }

    protected override void Attack(Zombie z)
    {
        StartCoroutine(Attack_Helper());
    }

    private IEnumerator Attack_Helper()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject g = Instantiate(projectile, transform.position + rightOffset, Quaternion.identity);
            g.transform.localScale = Tile.TILE_DISTANCE / 2;
            g.GetComponent<DestroyAfterAnimation>().lifetime = 0.2f;
            g = Instantiate(projectile, transform.position + rightOffset + topOffset, Quaternion.identity);
            g.transform.localScale = Tile.TILE_DISTANCE / 2;
            g.GetComponent<DestroyAfterAnimation>().lifetime = 0.2f;
            g = Instantiate(projectile, transform.position + topOffset, Quaternion.identity);
            g.transform.localScale = Tile.TILE_DISTANCE / 2;
            g.GetComponent<DestroyAfterAnimation>().lifetime = 0.2f;
            g = Instantiate(projectile, transform.position - rightOffset + topOffset, Quaternion.identity);
            g.transform.localScale = Tile.TILE_DISTANCE / 2;
            g.GetComponent<DestroyAfterAnimation>().lifetime = 0.2f;
            g = Instantiate(projectile, transform.position - rightOffset, Quaternion.identity);
            g.transform.localScale = Tile.TILE_DISTANCE / 2;
            g.GetComponent<DestroyAfterAnimation>().lifetime = 0.2f;
            g = Instantiate(projectile, transform.position - rightOffset - topOffset, Quaternion.identity);
            g.transform.localScale = Tile.TILE_DISTANCE / 2;
            g.GetComponent<DestroyAfterAnimation>().lifetime = 0.2f;
            g = Instantiate(projectile, transform.position - topOffset, Quaternion.identity);
            g.transform.localScale = Tile.TILE_DISTANCE / 2;
            g.GetComponent<DestroyAfterAnimation>().lifetime = 0.2f;
            g = Instantiate(projectile, transform.position + rightOffset - topOffset, Quaternion.identity);
            g.transform.localScale = Tile.TILE_DISTANCE / 2;
            g.GetComponent<DestroyAfterAnimation>().lifetime = 0.2f;
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, Tile.TILE_DISTANCE * 3, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie", "Shield"));
            foreach (RaycastHit2D r in hits)
            {
                if (r.collider.offset.y != 0) continue;
                r.collider.GetComponent<Damagable>().ReceiveDamage(damage, null);
            }
            yield return new WaitForSeconds(0.25f);
        }
        base.Attack(null);
    }

}
