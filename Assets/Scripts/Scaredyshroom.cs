using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaredyshroom : Peashooter
{

    public Vector2 scareRange;

    public Sprite hidingSprite;
    private Sprite normalSprite;

    public override void Start()
    {
        normalSprite = SR.sprite;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, scareRange * Tile.TILE_DISTANCE, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie"));
        if (!hit)
        {
            SR.sprite = normalSprite;
            base.Update();
        }
        else SR.sprite = hidingSprite;
    }

}
