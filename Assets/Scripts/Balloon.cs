using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : Zombie
{

    public float poppedWalkTime;
    private bool popped;

    protected override void Walk()
    {
        if (popped) base.Walk();
        else WalkConstant();
    }

    public override void ReceiveDamage(float dmg, GameObject source, bool eat = false)
    {
        if (!popped && source != null && source.GetComponent<StraightProjectile>() != null && source.GetComponent<StraightProjectile>().sharp)
        {
            popped = true;
            BC.offset = new Vector2(0, 0);
            BC.size = new Vector2(1, 1);
            walkTime = poppedWalkTime;
            ResetWalk();
        }
        base.ReceiveDamage(dmg, source, eat);
    }

    protected override IEnumerator Eat(Damagable p)
    {
        if (!popped) yield break;
        while (true) yield return base.Eat(p);
    }

}
