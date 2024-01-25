using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jalapeno : Plant
{

    protected override void Attack(Zombie z)
    {
        Vector3 area = new Vector3(9, 0.75f);
        GameObject g = Instantiate(projectile, new Vector2(Tile.COL_TO_WORLD[5], transform.position.y - (0.5f - area.y / 2) * Tile.TILE_DISTANCE.y), Quaternion.identity);
        g.transform.localScale = area * Tile.TILE_DISTANCE;
        RaycastHit2D[] all = Physics2D.BoxCastAll(g.transform.position, area * Tile.TILE_DISTANCE, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie"));
        foreach (RaycastHit2D a in all)
        {
            a.collider.GetComponent<Zombie>().ReceiveDamage(damage, gameObject);
        }
        for (int c = 1; c < 10; c++)
        {
            GameObject t = Tile.tileObjects[row, c].gridItem;
            if (t != null && t.tag == "Snow") Destroy(t);
        }
        Destroy(gameObject);
    }

}
