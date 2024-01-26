using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoMine : Plant
{

    /// <summary> The amount of time in seconds to activate the mine. Will not call the base <c>Update</c> until activated </summary>
    public float armingTime;
    private Vector2 area = new Vector2(Tile.TILE_DISTANCE.x * 1.5f, Tile.TILE_DISTANCE.y * 0.75f);

    // Update is called once per frame
    public override void Update()
    {
        if (armingTime > 0)
        {
            armingTime -= Time.deltaTime;
            SR.material.color = Color.white * 0.75f;
        }
        else
        {
            base.Update();
            SR.material.color = Color.white;
        }
    }

    /// <summary> Explodes in a little over a 1x1 area, and then disappears </summary>
    protected override void Attack(Zombie z)
    {
        GameObject g = Instantiate(projectile, transform.position, Quaternion.identity);
        g.transform.localScale = area;
        RaycastHit2D[] all = Physics2D.BoxCastAll(transform.position, area, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie"));
        foreach (RaycastHit2D a in all)
        {
            a.collider.GetComponent<Zombie>().ReceiveDamage(damage, null);
        }
        z.ReceiveDamage(damage, null);
        Destroy(gameObject);
    }

    public void ForceAttack(Zombie z)
    {
        if (armingTime <= 0) Attack(z);
    }

}
