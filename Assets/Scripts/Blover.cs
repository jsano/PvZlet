using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blover : Plant
{

    private bool active;

    protected override void Attack(Zombie z)
    {
        if (active) return;
        active = true;
        StartCoroutine(Blow());
    }

    private IEnumerator Blow()
    {
        for (int i = 1; i <= ZS.lanes; i++)
        {
            for (int j = 1; j <= 9; j++)
            {
                if (Tile.tileObjects[i, j].fog != null) Tile.tileObjects[i, j].fog.Clear(45);
            }
        }
        Zombie[] balloons = FindObjectsByType<Balloon>(FindObjectsSortMode.None);
        foreach (Zombie target in balloons) target.ReceiveDamage(1000, gameObject);
        float period = 0;
        while (period < 1f)
        {
            List<GameObject> prev = new List<GameObject>();
            for (int c = 1; c < 10; c++)
            {
                RaycastHit2D[] all = Physics2D.BoxCastAll(Tile.tileObjects[row, c].transform.position, Tile.TILE_DISTANCE * new Vector2(1, 2), 0, Vector2.zero, 0, LayerMask.GetMask("Zombie", "ExplosivesOnly"));
                foreach (RaycastHit2D a in all)
                {
                    if (prev.Contains(a.collider.gameObject)) continue;
                    if (a.collider.GetComponent<Digger>() != null || a.collider.GetComponent<Bungee>() != null) continue;
                    else
                    {
                        if (row == a.collider.GetComponent<Zombie>().row)
                        {
                            int c1 = Mathf.Clamp(Tile.WORLD_TO_COL(a.collider.transform.position.x), 2, 9);
                            a.collider.transform.Translate((Tile.tileObjects[row, c1].transform.position - Tile.tileObjects[row, c1 - 1].transform.position) * 3 * Time.deltaTime);
                            prev.Add(a.collider.gameObject);
                        }
                    }
                }
            }
            period += Time.deltaTime;
            yield return null;
        }
    }

}
