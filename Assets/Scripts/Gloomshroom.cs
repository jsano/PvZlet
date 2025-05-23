using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloomshroom : Plant
{

    private Vector2 area = new Vector2(Tile.TILE_DISTANCE.x * 3f, Tile.TILE_DISTANCE.y * 2.5f);
    public AudioClip[] hitSFX;

    public override void Start()
    {
        base.Start();
        rightOffset = new Vector3(Tile.TILE_DISTANCE.x, 0, 0);
        topOffset = new Vector3(0, Tile.TILE_DISTANCE.y, 0);
    }

    protected override Zombie LookInRange(int row)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, area, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie"));
        foreach (RaycastHit2D hit in hits) if (hit.collider.GetComponent<BoxCollider2D>().size.y == 1) return hit.collider.GetComponent<Zombie>();
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
            SFX.Instance.Play(hitSFX[Random.Range(0, hitSFX.Length)]);
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
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, area, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie", "Shield"));
            foreach (RaycastHit2D r in hits)
            {
                if (r.collider.GetComponent<BoxCollider2D>().size.y != 1) continue;
                r.collider.GetComponent<Damagable>().ReceiveDamage(damage, gameObject, disintegrating: true);
            }
            yield return new WaitForSeconds(0.25f);
        }
        base.Attack(null);
    }

}
