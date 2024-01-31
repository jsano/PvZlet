using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bungee : Zombie
{

    private bool ret;
    private float startHeight = Tile.TILE_DISTANCE.y * 10;
    private int col;
    private GameObject target;

    // Update is called once per frame
    public override void Update()
    {
        if (!ret) StartCoroutine(Update_Helper());
    }

    private IEnumerator Update_Helper() 
    {
        ret = true;
        target = Instantiate(projectile, transform.position, Quaternion.identity);
        while (target.transform.position.y > Tile.tileObjects[row, col].transform.position.y)
        {
            target.transform.Translate(Vector3.down * 12 * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        while (transform.position.y > Tile.tileObjects[row, col].transform.position.y + Tile.TILE_DISTANCE.y / 3)
        {
            RB.velocity = Vector3.down * 15;
            yield return null;
        }
        BC.enabled = true;
        RB.velocity = Vector3.zero;
        yield return new WaitForSeconds(3f);
        BC.enabled = false;
        Tile.tileObjects[row, col].RemoveAllPlants();
        RB.velocity = Vector3.up * 15;
        while (transform.position.y < startHeight)
        {
            target.transform.Translate(Vector3.up * 15 * Time.deltaTime);
            yield return null;
        }
        Die();
    }

    protected override void Spawn()
    {
        row = -1;
        for (int i = 1; i <= ZS.lanes; i++)
        {
            if (row != -1) break;
            for (int j = 1; j <= 6; j++)
            {
                if (Tile.tileObjects[i, j].GetEatablePlant() != null)
                {
                    row = i;
                    col = j;
                    break;
                }
            }
        }
        if (row == -1)
        {
            row = Random.Range(1, ZS.lanes + 1);
            col = Random.Range(1, 7);
        }
        transform.position = new Vector3(Tile.COL_TO_WORLD[col], Tile.tileObjects[row, 9].transform.position.y + startHeight, 0);
    }

    protected override void Die()
    {
        Destroy(target);
        base.Die();
    }

}
