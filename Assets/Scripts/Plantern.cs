using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plantern : Plant
{

    // Update is called once per frame
    public override void Update()
    {
        for (int i = Mathf.Max(1, row - 2); i <= Mathf.Min(row + 2, ZS.lanes); i++)
            for (int j = Mathf.Max(1, col - 2); j <= Mathf.Min(col + 2, 9); j++)
            {
                if (Tile.tileObjects[i, j].fog != null)
                {
                    SpriteRenderer sr = Tile.tileObjects[i, j].fog.GetComponent<SpriteRenderer>();
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
                }
            }
        base.Update();
    }

    protected override void Attack(Zombie z)
    {
        for (int i = Mathf.Max(1, row - 1); i <= Mathf.Min(row + 1, ZS.lanes); i++)
            for (int j = Mathf.Max(1, col - 1); j <= Mathf.Min(col + 1, 9); j++)
                if (Tile.tileObjects[i, j].planted != null) Tile.tileObjects[i, j].planted.GetComponent<Plant>().Heal(1);
    }

    protected override void Die()
    {
        for (int i = Mathf.Max(1, row - 2); i <= Mathf.Min(row + 2, ZS.lanes); i++)
            for (int j = Mathf.Max(1, col - 2); j <= Mathf.Min(col + 2, 9); j++)
                if (Tile.tileObjects[i, j].fog != null)
                {
                    SpriteRenderer sr = Tile.tileObjects[i, j].fog.GetComponent<SpriteRenderer>();
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
                }
        base.Die();
    }

}
