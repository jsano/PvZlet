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
            RaycastHit2D[] all = Physics2D.BoxCastAll(g.transform.position, Tile.TILE_DISTANCE * new Vector2(1, 2), 0, Vector2.zero, 0, LayerMask.GetMask("Zombie", "ExplosivesOnly"));
            foreach (RaycastHit2D a in all)
            {
                if (prev.Contains(a.collider.gameObject)) continue;
                // Remove active ladders
                if (a.collider.GetComponent<Shield>() != null) Destroy(a.collider.gameObject);
                else
                {
                    if (row == a.collider.GetComponent<Zombie>().row)
                    {
                        a.collider.GetComponent<Zombie>().ReceiveDamage(damage, gameObject);
                        prev.Add(a.collider.gameObject);
                    }
                }
            }
            GameObject snow = Tile.tileObjects[row, c].ContainsGridItem("Snow");
            Destroy(snow);
        }
        Destroy(gameObject);
    }

}
