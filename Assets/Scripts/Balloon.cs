using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : Zombie
{

    public float poppedWalkTime;
    private bool popped;

    public override void Update()
    {
        if (!popped && status != null) status.walkMod = Mathf.Max(status.walkMod, 0.5f);
        base.Update();
    }

    protected override void Spawn()
    {
        base.Spawn();
        transform.position += new Vector3(0, Tile.TILE_DISTANCE.y / 2);
    }

    protected override void Walk()
    {
        if (popped) base.Walk();
        else WalkConstant();
    }

    public override float ReceiveDamage(float dmg, GameObject source, bool eat = false, bool disintegrating = false)
    {
        base.ReceiveDamage(dmg, source, eat, disintegrating);
        if (!popped && (armor == null || armor.GetComponent<Armor>().HP <= 0))
        {
            popped = true;
            BC.offset = new Vector2(0, 0);
            BC.size = new Vector2(1, 1);
            transform.position = new Vector3(transform.position.x, Tile.tileObjects[row, Tile.WORLD_TO_COL(transform.position.x)].transform.position.y);
            walkTime = poppedWalkTime;
            ResetWalk();
        }
        return HP;
    }

    protected override IEnumerator Eat(Damagable p)
    {
        if (!popped && p.GetComponent<Player>() == null) yield break;
        while (true) yield return base.Eat(p);
    }

}
