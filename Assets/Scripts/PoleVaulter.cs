using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleVaulter : Zombie
{

    public float noPoleWalkTime;
    private bool running = true;
    private bool jumped = false;
    private float jumpTime = 1;


    // Update is called once per frame
    public override void Update()
    {
        if (running)
        {
            WalkConstant();
            RaycastHit2D hit = Physics2D.BoxCast(transform.position - new Vector3(Tile.TILE_DISTANCE.x / 3, 0, 0), transform.localScale, 0, Vector2.zero, 0, LayerMask.GetMask("Plant"));
            if (hit)
            {
                running = false;
                walkTime = noPoleWalkTime;
                StartCoroutine(Jump());
            }
        }
        else
        {
            if (jumped) base.Update();
        }
    }

    private IEnumerator Jump()
    {
        RB.velocity = new Vector3(-Tile.TILE_DISTANCE.x * 1.5f / jumpTime, 0, 0); // d = rt
        yield return new WaitForSeconds(jumpTime);
        RB.velocity = Vector3.zero;
        jumped = true;
    }

}
