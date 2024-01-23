using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaredyshroom : Peashooter
{

    public Vector2 scareRange;

    // Update is called once per frame
    public override void Update()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, scareRange * Tile.TILE_DISTANCE, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie"));
        if (!hit)
        {
            SR.material.color = Color.white;
            base.Update();
        }
        else SR.material.color = Color.white / 2; // will cause jank but it's ok since this will be replaced with animations
    }

}
