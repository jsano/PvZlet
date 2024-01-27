using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starfruit : Plant
{

    protected override Zombie LookInRange(int row)
    {
        float radius = projectile.transform.localScale.x * projectile.GetComponent<CircleCollider2D>().radius;
        RaycastHit2D hit = Physics2D.CircleCast(Tile.tileObjects[row, col].transform.position, radius, Vector2.left, 10 * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
        if (!hit) hit = Physics2D.CircleCast(Tile.tileObjects[row, col].transform.position, radius, new Vector2(1, 0.5f), 10 * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
        if (!hit) hit = Physics2D.CircleCast(Tile.tileObjects[row, col].transform.position, radius, new Vector2(1, -0.5f), 10 * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
        if (!hit) hit = Physics2D.CircleCast(Tile.tileObjects[row, col].transform.position, radius, Vector2.up, 10 * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
        if (!hit) hit = Physics2D.CircleCast(Tile.tileObjects[row, col].transform.position, radius, Vector2.down, 10 * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
        if (hit) return hit.collider.GetComponent<Zombie>();
        return null;
    }

    protected override void Attack(Zombie z)
    {
        StraightProjectile p = Instantiate(projectile, transform.position, projectile.transform.rotation).GetComponent<StraightProjectile>();
        p.dir = Vector2.left;
        p = Instantiate(projectile, transform.position, projectile.transform.rotation).GetComponent<StraightProjectile>();
        p.dir = new Vector2(1, 0.5f);
        p = Instantiate(projectile, transform.position, projectile.transform.rotation).GetComponent<StraightProjectile>();
        p.dir = new Vector2(1, -0.5f);
        p = Instantiate(projectile, transform.position, projectile.transform.rotation).GetComponent<StraightProjectile>();
        p.dir = Vector2.up;
        p = Instantiate(projectile, transform.position, projectile.transform.rotation).GetComponent<StraightProjectile>();
        p.dir = Vector2.down;
    }

}
