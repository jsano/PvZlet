using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobsled : Zombie
{

    public GameObject zombie;

    // Update is called once per frame
    public override void Update()
    {
        int c = Tile.WORLD_TO_COL(transform.position.x);
        if (c != 0 && (Tile.tileObjects[row, c].gridItem == null || Tile.tileObjects[row, c].gridItem.tag != "Snow"))
        {
            Zombie z = Instantiate(zombie, transform.position, Quaternion.identity).GetComponent<Zombie>();
            z.row = row;
            z = Instantiate(zombie, transform.position + new Vector3(Tile.TILE_DISTANCE.x / 3, 0, 0), Quaternion.identity).GetComponent<Zombie>();
            z.row = row;
            z = Instantiate(zombie, transform.position + new Vector3(Tile.TILE_DISTANCE.x * 2 / 3, 0, 0), Quaternion.identity).GetComponent<Zombie>();
            z.row = row; 
            z = Instantiate(zombie, transform.position + new Vector3(Tile.TILE_DISTANCE.x, 0, 0), Quaternion.identity).GetComponent<Zombie>();
            z.row = row;
            ZS.currentBuild += 4;
            Die();
        }
        WalkConstant();
    }

}
