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
        float period = 0;
        while (period < 1.5f)
        {
            period += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
            yield return null;
        }
        int c = Tile.WORLD_TO_COL(transform.position.x);
        if (c >= 1 && c <= 9) Tile.tileObjects[row, c].RemoveAllPlants(gameObject);
        period = 0;
        while (period < 0.5f)
        {
            period += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
            yield return null;
        }
        attacking = false;
    }

    private IEnumerator Throw()
    {
        throwing = true;
        float period = 0;
        while (period < 0.75f)
        {
            period += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
            yield return null;
        }
        GameObject g = Instantiate(imp, transform.position + new Vector3(0, Tile.TILE_DISTANCE.y, 0), Quaternion.identity);
        g.GetComponent<Imp>().flung = true;
        g.GetComponent<Imp>().row = row;
        throwing = false;
    }

}
