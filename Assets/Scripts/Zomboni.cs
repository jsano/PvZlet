using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zomboni : Football
{

    // Update is called once per frame
    public override void Update()
    {
        int c = Tile.WORLD_TO_COL(transform.position.x);
        if (c != 0 && (Tile.tileObjects[row, c].gridItem == null || Tile.tileObjects[row, c].gridItem.tag != "Snow")) Tile.tileObjects[row, c].Place(projectile);
        base.Update();
    }

}
