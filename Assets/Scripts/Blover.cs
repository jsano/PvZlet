using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blover : Plant
{

    private bool active;
    public float pushTime;

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
        while (pushTime > 0)
        {
            RaycastHit2D[] all = Physics2D.BoxCastAll(Tile.tileObjects[row, 1].transform.position, Tile.TILE_DISTANCE, 0, Vector2.right, 9 * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
            foreach (RaycastHit2D a in all)
            {
                if (a.collider.GetComponent<Balloon>() != null) a.collider.GetComponent<Balloon>().ReceiveDamage(1000, gameObject);
                else a.collider.transform.Translate(new Vector3(Tile.TILE_DISTANCE.x * 2 * Time.deltaTime, 0, 0));
            }
            pushTime -= Time.deltaTime;
            yield return null;
        }
    }

}
