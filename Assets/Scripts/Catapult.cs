using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : Football
{

    public int count;
    public float throwInterval;
    private float throwPeriod;

    public AudioClip shoot;
    public AudioClip explosion;

    // Update is called once per frame
    public override void Update()
    {
        Tile target = null;
        for (int i = 9; i >= 1; i--) {
            GameObject p = Tile.tileObjects[row, i].GetEatablePlant(true);
            if (p != null)
            {
                target = Tile.tileObjects[row, i];
                break;
            }
        }
        base.Update();
        if (!(count == 0 || target == null || transform.position.x > Tile.tileObjects[row, 9].transform.position.x))
        {
            ResetWalk();
            throwPeriod += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
            if (throwPeriod >= throwInterval)
            {
                SFX.Instance.Play(shoot);
                throwPeriod = 0;
                LobbedProjectile p = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<LobbedProjectile>();
                p.distance = target.transform.position - transform.position;
                p.lane = row;
                count -= 1;
            }
        }
    }

    public override void Die()
    {
        SFX.Instance.Play(explosion);
        base.Die();
    }

}
