using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetshroom : Plant
{

    public float recoverTime;
    private float recoverPeriod;
    public Vector2 area;
    private GameObject taken;

    public override void Start()
    {
        recoverPeriod = recoverTime;
        base.Start();
    }

    public override void Update()
    {
        recoverPeriod += Time.deltaTime;
        if (recoverPeriod > recoverTime)
        {
            Destroy(taken);
            SR.material.color = Color.white;
            base.Update();
        }
    }

    protected override void Attack(Zombie z)
    {
        bool took = false;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, area * Tile.TILE_DISTANCE, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie", "Underground", "Default"));
        foreach (RaycastHit2D hit in hits)
        {
            z = hit.collider.GetComponent<Zombie>();
            if (hit.collider.GetComponent<Shield>() != null)
            {
                hit.transform.parent.GetComponent<Tile>().ladder = null;
                Take(hit.collider.gameObject);
                took = true;
            }
            else if (z.projectile != null && z.projectile.tag == "Metal")
            {
                Take(z.projectile);
                z.projectile = null;
                took = true;
            }
            else if (z.shield != null && z.shield.tag == "Metal")
            {
                Take(z.shield);
                z.shield = null;
                took = true;
            }
            else if (z.armor != null && z.armor.tag == "Metal")
            {
                z.armor.GetComponent<Armor>().DetachUser();
                Take(z.armor);
                z.armor = null;
                took = true;
            }
            if (took)
            {
                recoverPeriod = 0;
                SR.material.color -= Color.white / 2;
                break;
            }
        }
    }
        

    private void Take(GameObject g)
    {
        taken = g;
        g.transform.SetParent(transform);
        g.transform.localPosition = Vector3.zero;
        if (g.GetComponent<Collider2D>() != null) g.GetComponent<Collider2D>().enabled = false;
    }

}
