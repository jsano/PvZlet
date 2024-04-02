using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torchwood : Plant
{

    public GameObject pea;
    public GameObject firePea;

    private List<StraightProjectile> affected = new List<StraightProjectile>();

    public override void Update()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, Tile.TILE_DISTANCE / 2, 0, Vector2.zero, 0, LayerMask.GetMask("StraightProjectile"));
        foreach (RaycastHit2D hit in hits)
        {
            StraightProjectile orig = hit.collider.GetComponent<StraightProjectile>();
            if (affected.Exists(x => x == orig)) continue;
            if (orig.pea)
            {
                StraightProjectile p;
                if (hit.collider.GetComponent<ChillPea>() != null) p = Instantiate(pea, hit.transform.position, Quaternion.identity).GetComponent<StraightProjectile>();
                else p = Instantiate(firePea, hit.transform.position, Quaternion.identity).GetComponent<StraightProjectile>();
                p.Setup(orig.GetParent(), orig.GetDir(), orig.GetMoveToLane());
                affected.Add(p);
                Destroy(hit.collider.gameObject);
            }
        }
        if (hits.Length == 0) affected.Clear();
        base.Update();
    }

}
