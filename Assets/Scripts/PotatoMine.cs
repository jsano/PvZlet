using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoMine : Plant
{

    /// <summary> The amount of time in seconds to activate the mine. Will not call the base <c>Update</c> until activated </summary>
    public float armingTime;
    private Vector2 area = new Vector2(Tile.TILE_DISTANCE.x * 1.5f, Tile.TILE_DISTANCE.y * 0.75f);

    public Sprite armingSprite;
    private Sprite normalSprite;

    public override void Start()
    {
        normalSprite = SR.sprite;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (armingTime > 0)
        {

            armingTime -= Time.deltaTime;
            SR.sprite = armingSprite;
        }
        else
        {
            base.Update();
            HP = 100000;
            SR.sprite = normalSprite;
        }
    }

    protected override Zombie LookInRange(int row)
    {
        RaycastHit2D hit = Physics2D.Raycast(Tile.tileObjects[row, col].transform.position, Vector2.left, (backwardsRange + 0.5f) * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie", "ExplosivesOnly"));
        if (!hit) hit = Physics2D.Raycast(Tile.tileObjects[row, col].transform.position, Vector2.right, (range + 0.5f) * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie", "ExplosivesOnly"));
        if (hit) return hit.collider.GetComponent<Zombie>();
        return null;
    }

    /// <summary> Explodes in a little over a 1x1 area, and then disappears </summary>
    protected override void Attack(Zombie z)
    {
        GameObject g = Instantiate(projectile, transform.position, Quaternion.identity);
        g.transform.localScale = area;
        RaycastHit2D[] all = Physics2D.BoxCastAll(transform.position, area, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie", "ExplosivesOnly"));
        foreach (RaycastHit2D a in all)
        {
            a.collider.GetComponent<Zombie>().ReceiveDamage(damage, null, disintegrating: true);
        }
        //z.ReceiveDamage(damage, null);
        Destroy(gameObject);
    }

}
