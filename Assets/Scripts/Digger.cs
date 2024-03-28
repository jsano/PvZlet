using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : Zombie
{

    public float surfaceWalkTime;
    private bool digging = true;
    private float dazedTime = 5;
    private bool removed;

    public override void Start()
    {
        base.Start();
        projectile = Instantiate(projectile, transform, false);
        projectile.transform.localPosition = new Vector3(-transform.localScale.x / 2, 0, 0);
        projectile.GetComponent<SpriteRenderer>().sortingOrder = SR.sortingOrder + 2;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (projectile == null && !removed && digging)
        {
            removed = true;
            dazedTime /= 2;
            walkTime = surfaceWalkTime;
            StartCoroutine(Rise());
            ResetWalk();
        }
        if (removed)
        {
            dazedTime = Mathf.Max(0, dazedTime - Time.deltaTime);
            if (dazedTime <= 0) base.Update();
            return;
        }

        if (transform.position.x <= Tile.COL_TO_WORLD[1] - Tile.TILE_DISTANCE.x / 4)
        {
            digging = false;
            gameObject.layer = LayerMask.NameToLayer("Zombie");
            RB.velocity = Vector3.zero;
            backwards = true;
            walkTime = surfaceWalkTime;
        }
        if (digging) WalkConstant();
        else dazedTime = Mathf.Max(0, dazedTime - Time.deltaTime);
        if (dazedTime <= 0) base.Update();
    }

    private IEnumerator Rise()
    {
        yield return new WaitUntil(() => dazedTime <= 0);
        gameObject.layer = LayerMask.NameToLayer("Zombie");
    }


}
