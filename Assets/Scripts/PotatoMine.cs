using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoMine : Plant
{

    public float armingTime;

    // Update is called once per frame
    public override void Update()
    {
        if (armingTime > 0) armingTime -= Time.deltaTime;
        else
        {
            base.Update();
            GetComponent<SpriteRenderer>().color = Color.magenta;
        }
    }

    protected override void Attack()
    {
        GameObject g = Instantiate(projectile, transform.position, Quaternion.identity);
        g.transform.localScale = Tile.TILE_DISTANCE;
        RaycastHit2D[] all = Physics2D.BoxCastAll(transform.position, Tile.TILE_DISTANCE, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie"));
        foreach (RaycastHit2D a in all)
        {
            a.collider.GetComponent<Zombie>().ReceiveDamage(damage);
        }
        Destroy(gameObject);
    }

}
