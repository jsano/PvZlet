using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bungee : Zombie
{

    private bool ret;
    private float startHeight = Tile.TILE_DISTANCE.y * 10;
    [HideInInspector] public int col;
    private GameObject target;

    public AudioClip[] descend;
    public AudioClip take;

    // Update is called once per frame
    public override void Update()
    {
        if (!ret) StartCoroutine(Update_Helper());
    }

    private IEnumerator Update_Helper() 
    {
        ret = true;
        gameObject.layer = LayerMask.NameToLayer("ExplosivesOnly");
        target = Instantiate(projectile, transform.position, Quaternion.identity);
        while (target.transform.position.y > Tile.tileObjects[row, col].transform.position.y)
        {
            target.transform.Translate(Vector3.down * 12 * Time.deltaTime);
            yield return null;
        }
        SFX.Instance.Play(descend[Random.Range(0, descend.Length)]);
        yield return new WaitForSeconds(1f);
        while (transform.position.y > Tile.tileObjects[row, col].transform.position.y)
        {
            RB.velocity = Vector3.down * 15;
            if (transform.position.y - Tile.tileObjects[row, col].transform.position.y <= Tile.TILE_DISTANCE.y)
                gameObject.layer = LayerMask.NameToLayer("Zombie");
            yield return null;
        }
        RB.velocity = Vector3.zero;
        float timeLeft = 3f;
        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime * ((status == null) ? 1 : status.eatMod);
            yield return null;
        }
        SFX.Instance.Play(take);
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
        for (int i = 1; i <= ZombieSpawner.Instance.lanes; i++)
        {
            if (row != 0 && i != row) continue;
            for (int j = 1; j <= ZombieSpawner.Instance.lanes; j++)
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
        if (row == 0) row = Random.Range(1, ZombieSpawner.Instance.lanes + 1);
        if (col == 0) col = Random.Range(1, 7);
        transform.position = new Vector3(Tile.COL_TO_WORLD[col], Tile.tileObjects[row, col].transform.position.y + startHeight, 0);
    }

    public override void Die()
    {
        Destroy(target);
        base.Die();
    }

}
