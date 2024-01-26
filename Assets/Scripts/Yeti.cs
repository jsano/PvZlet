using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yeti : Zombie
{

    public float forwardTime;

    public override void LateUpdate()
    {
        base.LateUpdate();
        forwardTime -= Time.deltaTime;
        if (forwardTime <= 0 && RB.velocity.x < 0) RB.velocity *= -1;
        if (forwardTime <= 0 && Tile.COL_TO_WORLD[9] + Tile.TILE_DISTANCE.x <= transform.position.x && !hypnotized) base.Die();
        // NOTE: Currently drops money offscreen when hypnotized
    }

    protected override void Die()
    {
        Debug.Log("Lots of money");
        base.Die();
    }

}
