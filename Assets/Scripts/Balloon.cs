using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : Zombie
{

    private bool popped;

    public AudioClip popSFX;

    public override void Start()
    {
        projectile = Instantiate(projectile, transform, false);
        projectile.transform.localPosition = new Vector3(0, BC.size.y / 2, 0);
        projectile.GetComponent<SpriteRenderer>().sortingOrder = SR.sortingOrder + 2;
        base.Start();
        
    }

    public override void Update()
    {
        if (projectile != null && status != null) status.walkMod = Mathf.Max(status.walkMod, 0.5f);
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
        if (projectile == null) base.Walk();
        else WalkConstant();
    }

    public override float ReceiveDamage(float dmg, GameObject source, bool eat = false, bool disintegrating = false)
    {
        base.ReceiveDamage(dmg, source, eat, disintegrating);
        if (projectile != null && source.GetComponent<StraightProjectile>() != null && source.GetComponent<StraightProjectile>().sharp)
        {
            if (!disintegrating) SFX.Instance.Play(popSFX);
            Destroy(projectile);
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
        if (projectile != null && p.GetComponent<Player>() == null) return;
        base.Eat(p);
    }

}
