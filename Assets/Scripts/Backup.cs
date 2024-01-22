using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backup : Zombie
{

    [HideInInspector] public float xLoc;
    [HideInInspector] public bool beingSpawned = true;

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
        transform.position = new Vector3(xLoc, Tile.ROW_TO_WORLD[row], 0);
    }

    public void StopForOthers()
    {
        RB.velocity = Vector3.zero;
        period = 0;
    }

}
