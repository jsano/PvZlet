using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : Zombie
{

    private bool popped;

    public AudioClip popSFX;

    public override void Update()
    {
        if (!popped && status != null) status.walkMod = Mathf.Max(status.walkMod, 0.5f);
        base.Update();
    }

    protected override void Spawn()
    {
        BC.size = new Vector2(1, 0.01f);
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
            if (!disintegrating) SFX.Instance.Play(popSFX);
            popped = true;
            BC.offset = new Vector2(0, 0);
            BC.size = new Vector2(1, 1);
            transform.position = new Vector3(transform.position.x, Tile.tileObjects[row, Mathf.Clamp(Tile.WORLD_TO_COL(transform.position.x), 1, 9)].transform.position.y);
            walkTime = alternateWalkTime[0];
            ResetWalk();
        }
        return HP;
    }

    protected override void Eat(GameObject p)
    {
        if (!popped && p.GetComponent<Player>() == null) return;
        base.Eat(p);
    }

}
