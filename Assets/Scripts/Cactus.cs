using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : Plant
{

    private float attack;
    private float baseY;

    private Sprite normalSprite;
    public Sprite longSprite;

    public AudioClip grow;

    public override void Start()
    {
        normalSprite = SR.sprite;
        base.Start();
        baseY = transform.position.y;
        attack = baseY;
    }

    public override void Update()
    {
        base.Update();
        transform.position = new Vector3(transform.position.x, (attack + baseY) / 2, 0);
    }

    protected override Zombie LookInRange(int row)
    {
        RaycastHit2D hit = Physics2D.BoxCast(Tile.tileObjects[row, col].transform.position + new Vector3(0, 3 * Tile.TILE_DISTANCE.y / 8, 0), new Vector2(0.01f, Tile.TILE_DISTANCE.y / 4), 0, Vector2.right, 0.5f * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
        for (int i = 1; i <= range && col + i <= 9 && !hit; i++)
            hit = Physics2D.BoxCast(Tile.tileObjects[row, col + i].transform.position + new Vector3(-Tile.TILE_DISTANCE.x / 2, 3 * Tile.TILE_DISTANCE.y / 8, 0), new Vector2(0.01f, Tile.TILE_DISTANCE.y / 4), 0, Vector2.right, Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
        if (hit && hit.collider.GetComponent<BoxCollider2D>().size != Vector2.one)
        {
            if (attack == baseY) SFX.Instance.Play(grow);
            SR.sprite = longSprite;
            attack = Mathf.Max(baseY, hit.transform.position.y);
            return hit.collider.GetComponent<Zombie>();
        }
        Zombie z = base.LookInRange(row);
        if (z == null)
        {
            attack = baseY;
            SR.sprite = normalSprite;
        }
        else
        {
            if (z.transform.position.y > baseY + Tile.TILE_DISTANCE.y / 2)
            {
                if (attack == baseY) SFX.Instance.Play(grow);
                SR.sprite = longSprite;
            }
            else SR.sprite = normalSprite;
            attack = Mathf.Max(baseY, z.transform.position.y);
        }
        return z;
    }

    protected override void Attack(Zombie z)
    {
        if (z != null)
        {
            StraightProjectile p = Instantiate(projectile, new Vector3(transform.position.x, attack) + rightOffset, projectile.transform.rotation).GetComponent<StraightProjectile>();
            p.Setup(gameObject, Vector3.right, blockAtSlopeColumn: Tile.WORLD_TO_COL(z.transform.position.x) + 1);
        }
        base.Attack(z);
    }

}
