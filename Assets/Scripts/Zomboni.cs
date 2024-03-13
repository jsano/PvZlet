using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zomboni : Football
{

    private int snowedCol = 10;

    // Update is called once per frame
    public override void Update()
    {
        int c = Tile.WORLD_TO_COL(transform.position.x + Tile.TILE_DISTANCE.x / 2);
        if (c <= 9 && c > 1 && c < snowedCol && (Tile.tileObjects[row, c].gridItem == null || Tile.tileObjects[row, c].gridItem.tag == "Snow"))
        {
            Tile.tileObjects[row, c].Place(projectile);
            snowedCol = c;
        }
        base.Update();
    }

}
