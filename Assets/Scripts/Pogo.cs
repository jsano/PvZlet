using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pogo : Zombie
{

    private GameObject toJump;
    public float noPogoWalkTime;
    private bool jumping;
    private bool removed;

    public override void Start()
    {
        base.Start();
        projectile = Instantiate(projectile, transform, false);
        projectile.transform.localPosition = new Vector3(0, -transform.localScale.y / 2, 0);
    }

    // Update is called once per frame
    public override void Update()
    {
        if (projectile != null && status != null) status.walkMod = Mathf.Max(status.walkMod, 0.5f);
        if (projectile != null && !jumping)
        {
            WalkConstant();
            toJump = ClosestEatablePlant(Physics2D.BoxCastAll(transform.position, transform.localScale, 0, Vector2.left, 0, LayerMask.GetMask("Plant")));
            if (toJump != null) StartCoroutine(Jump());
        }
        if (projectile == null && !removed)
        {
            StopAllCoroutines();
            ResetWalk();
            walkTime = noPogoWalkTime;
            removed = true;
        }
        if (removed) base.Update();
    }

    private IEnumerator Jump()
    {
        jumping = true;
        RB.velocity = Vector2.zero;
        Vector3 loc = toJump.transform.position;
        yield return new WaitForSeconds(1);
        RB.velocity = new Vector3(-3, 0, 0) * ((status == null) ? 1 : status.walkMod); // d = rt
        if (toJump != null && toJump.tag == "Tallnut")
        {
            yield return new WaitUntil(() => transform.position.x <= loc.x + Tile.TILE_DISTANCE.x / 3);
            Destroy(projectile);
        }
        else
        {
            yield return new WaitUntil(() => transform.position.x <= loc.x - Tile.TILE_DISTANCE.x / 2);
        }
        RB.velocity = Vector3.zero;
        jumping = false;
    }

}
