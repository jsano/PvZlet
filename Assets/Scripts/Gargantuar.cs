using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargantuar : Zombie
{

    public GameObject imp;
    private bool hasImp = true;
    private bool attacking;
    private bool throwing;

    // Update is called once per frame
    public override void Update()
    {
        if (!attacking && hasImp && HP <= baseHP / 3 && Tile.WORLD_TO_COL(transform.position.x) >= 5)
        {
            hasImp = false;
            ResetWalk();
            StartCoroutine(Throw());
        }
        if (!attacking && !throwing) base.Update();
    }

    protected override IEnumerator Eat(Damagable p)
    {
        ResetWalk();
        attacking = true;
        Plant p1 = p.GetComponent<Plant>();
        yield return new WaitForSeconds(1.5f);
        Tile.tileObjects[p1.row, p1.col].RemoveAllPlants(gameObject);
        yield return new WaitForSeconds(0.5f);
        attacking = false;
    }

    private IEnumerator Throw()
    {
        throwing = true;
        yield return new WaitForSeconds(1f);
        GameObject g = Instantiate(imp, transform.position + new Vector3(0, Tile.TILE_DISTANCE.y, 0), Quaternion.identity);
        g.GetComponent<Imp>().flung = true;
        g.GetComponent<Imp>().row = row;
        throwing = false;
    }

}
