using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleVaulter : Zombie
{

    public float noPoleWalkTime;
    private bool running = true;
    private bool jumped = false;
    private GameObject toJump;

    public override void Start()
    {
        if (projectile != null)
        {
            projectile = Instantiate(projectile, transform, false);
            projectile.transform.localPosition = new Vector3(0, transform.localScale.y / 2, 0);
        }
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (running)
        {
            WalkConstant();
            toJump = ClosestEatablePlant(Physics2D.BoxCastAll(transform.position - new Vector3(transform.localScale.x, 0, 0), new Vector2(0.1f, transform.localScale.y), 0, Vector2.left, 0, LayerMask.GetMask("Plant")));
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
        if (projectile != null && projectile.transform.localPosition.y > 0) // Pole only
        {
            projectile.transform.rotation = Quaternion.Euler(0, 0, 90);
            projectile.transform.localPosition = new Vector3(-transform.localScale.x / 2, 0, 0);
            projectile.transform.SetParent(null);
        }
        int c = Mathf.Clamp(Tile.WORLD_TO_COL(transform.position.x), 1, 9);
        if (c == 1) RB.velocity = (Tile.tileObjects[row, c].transform.position - Tile.tileObjects[row, c + 1].transform.position) * 2;
        else if (c == 2) RB.velocity = (Tile.tileObjects[row, c - 1].transform.position - Tile.tileObjects[row, c].transform.position) * 2;
        else RB.velocity = Tile.tileObjects[row, c - 2].transform.position - Tile.tileObjects[row, c].transform.position;
        Vector2 baseVel = RB.velocity / 1.75f; // d = rt
        gameObject.layer = LayerMask.NameToLayer("ExplosivesOnly");
        float period = 0;
        while (period < 1.5f)
        {
            RB.velocity = baseVel * ((status == null) ? 1 : status.walkMod);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 0.1f, LayerMask.GetMask("Plant"));
            if (hit && hit.collider.tag == "Tallnut")
            {
                RB.velocity = Vector3.zero;
                break;
            }
            period += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
            yield return null;
        }
        Destroy(projectile);
        gameObject.layer = LayerMask.NameToLayer("Zombie");
        RB.velocity = Vector3.zero;
        transform.position = new Vector2(transform.position.x, Tile.tileObjects[row, Mathf.Clamp(Tile.WORLD_TO_COL(transform.position.x), 1, 9)].transform.position.y);
        yield return new WaitForSeconds(0.5f);
        jumped = true;
    }

}
