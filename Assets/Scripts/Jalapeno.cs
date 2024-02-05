using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jalapeno : Plant
{

    protected override void Attack(Zombie z)
    {
        List<GameObject> prev = new List<GameObject>();
        for (int c = 1; c < 10; c++)
        {
            Vector2 diff = Tile.tileObjects[row, Mathf.Min(9, c + 1)].transform.position - Tile.tileObjects[row, c].transform.position;
            GameObject g = Instantiate(projectile, Tile.tileObjects[row, c].transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(diff.y, diff.x) * 180 / Mathf.PI));
            g.transform.localScale = Tile.TILE_DISTANCE + new Vector2(0.1f, 0);
            RaycastHit2D[] all = Physics2D.BoxCastAll(g.transform.position, g.transform.localScale, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie"));
            foreach (RaycastHit2D a in all)
            {
                if (prev.Contains(a.collider.gameObject)) continue;
                a.collider.GetComponent<Zombie>().ReceiveDamage(damage, gameObject);
                prev.Add(a.collider.gameObject);
            }
            GameObject t = Tile.tileObjects[row, c].gridItem;
            if (t != null && t.tag == "Snow") Destroy(t);
        }
        Destroy(gameObject);
    }

}
