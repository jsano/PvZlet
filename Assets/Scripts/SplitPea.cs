using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitPea : Peashooter
{

    protected override void Attack(Zombie z)
    {
        RaycastHit2D hit = Physics2D.Raycast(Tile.tileObjects[row, col].transform.position, Vector2.right, 0.5f * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
        for (int i = 1; i <= range && col + i <= 9 && !hit; i++)
        {
            hit = Physics2D.Raycast(Tile.tileObjects[row, col + i].transform.position - new Vector3(Tile.TILE_DISTANCE.x / 2, 0, 0), Vector2.right, Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
        }
        if (hit) base.Attack(z);

        hit = Physics2D.Raycast(Tile.tileObjects[row, col].transform.position, Vector2.left, 0.5f * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
        for (int i = 1; i <= backwardsRange && col - i >= 1 && !hit; i++)
        {
            hit = Physics2D.Raycast(Tile.tileObjects[row, col - i].transform.position + new Vector3(Tile.TILE_DISTANCE.x / 2, 0, 0), Vector2.left, Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
        }
        if (hit) StartCoroutine(Attack_Backwards(z));
    }

    private IEnumerator Attack_Backwards(Zombie z)
    {
        StraightProjectile p = Instantiate(projectile, transform.position - rightOffset, projectile.transform.rotation).GetComponent<StraightProjectile>();
        if (p.distance != range) p.distance = range;
        p.dir = Vector3.left;
        yield return new WaitForSeconds(0.2f);
        SFX.Instance.Play(Random.Range(0, 1f) < 0.5f ? attackSFX2 : attackSFX1);
        p = Instantiate(projectile, transform.position - rightOffset, projectile.transform.rotation).GetComponent<StraightProjectile>();
        if (p.distance != range) p.distance = range;
        p.dir = Vector3.left;
        base.Attack(null);
    }

}
