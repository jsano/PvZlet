using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Threepeater : Peashooter
{

    public override void Start()
    {
        base.Start();
        rightOffset = Vector3.zero;
    }

    protected override Zombie LookInRange(int r)
    {
        Zombie z = base.LookInRange(row);
        if (z == null && row < ZombieSpawner.Instance.lanes) z = base.LookInRange(row + 1);
        if (z == null && row > 1) z = base.LookInRange(row - 1);
        return z;
    }

    protected override void Attack(Zombie z)
    {
        if (row > 1)
        {
            StraightProjectile p = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<StraightProjectile>();
            p.Setup(gameObject, Vector3.right, row - 1);
        }
        else StartCoroutine(Attack_Helper(z));
        base.Attack(z);
        if (row < ZombieSpawner.Instance.lanes)
        {
            StraightProjectile p = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<StraightProjectile>();
            p.Setup(gameObject, Vector3.right, row + 1);
        }
        else StartCoroutine(Attack_Helper(z));
    }

    private IEnumerator Attack_Helper(Zombie z)
    {
        yield return new WaitForSeconds(0.2f);
        base.Attack(z);
    }

}
