using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bungee : Zombie
{

    private bool ret;
    private float startHeight = Tile.TILE_DISTANCE.y * 10;
    [HideInInspector] public int col;
    private GameObject target;

    // Update is called once per frame
    public override void Update()
    {
        if (!ret) StartCoroutine(Update_Helper());
    }

    private IEnumerator Update_Helper() 
    {
        BC.enabled = false;
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
        List<(int, int)> found = new List<(int, int)>();
        for (int i = 1; i <= ZS.lanes; i++)
        {
            if (row != 0 && i != row) continue;
            for (int j = 1; j <= ZS.lanes; j++)
            {
                if (col != 0 && j != col) continue;
                if (Tile.tileObjects[i, j].GetEatablePlant() != null)
                {
                    found.Add((i, j));
                }
            }
        }
        if (found.Count > 0)
        {
            int index = Random.Range(0, found.Count);
            row = found[index].Item1;
            col = found[index].Item2;
        }
        // No plant found
        if (row == 0) row = Random.Range(1, ZS.lanes + 1);
        if (col == 0) col = Random.Range(1, 7);
        transform.position = new Vector3(Tile.COL_TO_WORLD[col], Tile.tileObjects[row, col].transform.position.y + startHeight, 0);
    }

    public override void Die()
    {
        Destroy(target);
        base.Die();
    }

}
