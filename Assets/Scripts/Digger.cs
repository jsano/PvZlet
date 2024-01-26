using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : Zombie
{

    public float surfaceWalkTime;
    private bool digging = true;
    private float dazedTime = 5;

    // Update is called once per frame
    public override void Update()
    {
        if (transform.position.x <= Tile.COL_TO_WORLD[1] - Tile.TILE_DISTANCE.x / 2)
        {
            digging = false;
            BC.enabled = true;
            RB.velocity = Vector3.zero;
            backwards = true;
            walkTime = surfaceWalkTime;
        }
        if (digging) WalkConstant();
        else dazedTime = Mathf.Max(0, dazedTime - Time.deltaTime);
        if (dazedTime <= 0) base.Update();

        int c = Tile.WORLD_TO_COL(transform.position.x);
        if (c != 0) {
            GameObject g = Tile.tileObjects[row, c].ContainsPlant("PotatoMine");
            if (g != null) g.GetComponent<PotatoMine>().ForceAttack(this);
        }
    }

}
