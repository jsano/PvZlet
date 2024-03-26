using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleVaulter : Zombie
{

    public float noPoleWalkTime;
    private bool running = true;
    private bool jumped = false;
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
        Vector3 loc = toJump.transform.position;
        int c = Mathf.Clamp(Tile.WORLD_TO_COL(transform.position.x), 1, 8);
        RB.velocity = (Tile.tileObjects[row, c].transform.position - Tile.tileObjects[row, c + 1].transform.position) * ((status == null) ? 1 : status.walkMod); // d = rt
        if (toJump != null && toJump.tag == "Tallnut")
        {
            yield return new WaitUntil(() => transform.position.x <= loc.x + Tile.TILE_DISTANCE.x / 3);
            RB.velocity = Vector3.zero;
        }
        else
        {
            yield return new WaitUntil(() => transform.position.x <= loc.x - 2 * Tile.TILE_DISTANCE.x / 3);
            RB.velocity = Vector3.zero;
            yield return new WaitForSeconds(0.5f);
        }
        jumped = true;
    }

}
