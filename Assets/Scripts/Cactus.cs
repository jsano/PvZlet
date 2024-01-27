using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : Peashooter
{

    private float attack;
    private float baseY;

    public override void Start()
    {
        base.Start();
        baseY = transform.position.y;
    }

    public override void Update()
    {
        base.Update();
        transform.position = new Vector3(transform.position.x, attack, 0);
    }

    protected override Zombie LookInRange(int row)
    {
        RaycastHit2D hit = Physics2D.BoxCast(new Vector2(transform.position.x, Tile.ROW_TO_WORLD[row]), Tile.TILE_DISTANCE, 0, Vector2.right, (range + 0.5f) * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
        if (hit)
        {
            attack = hit.point.y;
            return hit.collider.GetComponent<Zombie>();
        }
        attack = baseY;
        return base.LookInRange(row);
    }

    protected override void Attack(Zombie z)
    {
        
        base.Attack(z);
    }

}
