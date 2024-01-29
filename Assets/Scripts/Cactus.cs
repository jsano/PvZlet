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
        RaycastHit2D hit = Physics2D.BoxCast(Tile.tileObjects[row, col].transform.position, new Vector2(0.01f, Tile.TILE_DISTANCE.y / 2), 0, Vector2.right, 0.5f * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
        for (int i = 1; i <= range && col + i <= 9 && !hit; i++)
            hit = Physics2D.BoxCast(Tile.tileObjects[row, col + i].transform.position - new Vector3(Tile.TILE_DISTANCE.x / 2, 0, 0), new Vector2(0.01f, Tile.TILE_DISTANCE.y / 2), 0, Vector2.right, Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
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
