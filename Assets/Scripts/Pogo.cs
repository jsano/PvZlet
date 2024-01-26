using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pogo : Zombie
{

    private GameObject toJump;
    public float noPogoWalkTime;
    private bool jumping;
    private Coroutine jumpCoroutine;

    // Update is called once per frame
    public override void Update()
    {
        if (status != null) status.walkMod = Mathf.Max(status.walkMod, 0.5f);
        if (projectile != null && !jumping)
        {
            WalkConstant();
            toJump = ClosestEatablePlant(Physics2D.BoxCastAll(transform.position, transform.localScale, 0, Vector2.left, 0, LayerMask.GetMask("Plant")));
            if (toJump != null) jumpCoroutine = StartCoroutine(Jump());
        }
        if (projectile == null)
        {
            StopCoroutine(jumpCoroutine);
            walkTime = noPogoWalkTime;
            base.Update();
        }
    }

    private IEnumerator Jump()
    {
        jumping = true;
        RB.velocity = Vector2.zero;
        yield return new WaitForSeconds(1);
        RB.velocity = new Vector3(-3, 0, 0) * ((status == null) ? 1 : status.walkMod); // d = rt
        if (toJump.tag == "Tallnut")
        {
            yield return new WaitUntil(() => transform.position.x <= toJump.transform.position.x + Tile.TILE_DISTANCE.x / 3);
            projectile = null;
        }
        else
        {
            yield return new WaitUntil(() => transform.position.x <= toJump.transform.position.x - Tile.TILE_DISTANCE.x / 2);
        }
        RB.velocity = Vector3.zero;
        jumping = false;
    }

}
