using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backup : Zombie
{

    [HideInInspector] public float xLoc;
    private bool beingSpawned = true;

    public override void Update()
    {
        if (beingSpawned) BC.enabled = false;
        else
        {
            BC.enabled = true;
            base.Update();
        }
    }

    protected override void Spawn()
    {
        transform.position = new Vector3(xLoc, Tile.tileObjects[row, Tile.WORLD_TO_COL(xLoc)].transform.position.y, 0);
        StartCoroutine(FinishSpawn());
    }

    private IEnumerator FinishSpawn()
    {
        yield return new WaitForSeconds(1.5f);
        beingSpawned = false;
    }

}
