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
            toJump = ClosestEatablePlant(Physics2D.BoxCastAll(transform.position - new Vector3(transform.localScale.x / 1.5f, 0, 0), new Vector2(0.1f, transform.localScale.y), 0, Vector2.left, 0, LayerMask.GetMask("Plant")));
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
        yield return new WaitForSeconds(1);
        int c = Mathf.Clamp(Tile.WORLD_TO_COL(transform.position.x), 1, 9);
        if (c == 1) RB.velocity = Tile.tileObjects[row, c].transform.position - Tile.tileObjects[row, c + 1].transform.position;
        else RB.velocity = Tile.tileObjects[row, c - 1].transform.position - Tile.tileObjects[row, c].transform.position;
        Vector2 baseVel = RB.velocity / 0.75f; // d = rt
        float period = 0;
        while (period < 0.75f)
        {
            RB.velocity = baseVel * ((status == null) ? 1 : status.walkMod);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 0.1f, LayerMask.GetMask("Plant"));
            if (hit && hit.collider.tag == "Tallnut")
            {
                Destroy(projectile);
                break;
            }
            period += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
            yield return null;
        }
        RB.velocity = Vector3.zero;
        transform.position = new Vector2(transform.position.x, Tile.tileObjects[row, Mathf.Clamp(Tile.WORLD_TO_COL(transform.position.x), 1, 9)].transform.position.y);
        jumping = false;
    }

}
