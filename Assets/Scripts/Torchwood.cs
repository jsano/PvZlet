using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torchwood : Plant
{

    public GameObject pea;
    public GameObject firePea;

    private List<StraightProjectile> affected = new List<StraightProjectile>();

    public override void Update()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, transform.localScale, 0, Vector2.zero, 0, LayerMask.GetMask("PlantProjectile"));
        foreach (RaycastHit2D hit in hits)
        {
            if (affected.Exists(x => x == hit.collider.GetComponent<StraightProjectile>())) continue;
            if (hit.collider.GetComponent<StraightProjectile>().pea)
            {
                if (hit.collider.GetComponent<ChillPea>() != null)
                {
                    GameObject p = Instantiate(pea, hit.transform.position, Quaternion.identity);
                    p.GetComponent<StraightProjectile>().distance = hit.collider.GetComponent<ChillPea>().distance;
                    affected.Add(p.GetComponent<StraightProjectile>());
                    Destroy(hit.collider.gameObject);
                }
                else
                {
                    GameObject p = Instantiate(firePea, hit.transform.position, Quaternion.identity);
                    p.GetComponent<StraightProjectile>().distance = hit.collider.GetComponent<StraightProjectile>().distance;
                    affected.Add(p.GetComponent<StraightProjectile>());
                    Destroy(hit.collider.gameObject);
                }
            }
        }
        if (hits.Length == 0) affected.Clear();
        base.Update();
    }

}
