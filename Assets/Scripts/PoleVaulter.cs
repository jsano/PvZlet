using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleVaulter : Zombie
{

    public float noPoleWalkTime;
    private bool running = true;
    private bool jumped = false;
    private float jumpTime = 1;
    private GameObject toJump;

    // Update is called once per frame
    public override void Update()
    {
        if (running)
        {
            WalkConstant();
            toJump = ClosestEatablePlant(Physics2D.BoxCastAll(transform.position, transform.localScale, 0, Vector2.left, Tile.TILE_DISTANCE.x / 2, LayerMask.GetMask("Plant")));
            if (toJump != null && (status == null || status.walkMod > 0))
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
        RB.velocity = new Vector3(-Tile.TILE_DISTANCE.x * 1.75f / jumpTime, 0, 0) * ((status == null) ? 1 : status.walkMod); // d = rt
        if (toJump.tag == "Tallnut")
        {
            yield return new WaitUntil(() => transform.position.x <= toJump.transform.position.x + Tile.TILE_DISTANCE.x / 3);
            RB.velocity = Vector3.zero;
        }
        else
        {
            yield return new WaitForSeconds(jumpTime * ((status == null) ? 1 : 1 / status.walkMod));
            RB.velocity = Vector3.zero;
            yield return new WaitForSeconds(0.5f);
        }
        jumped = true;
    }

}
