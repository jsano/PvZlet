using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hypnoshroom : Plant
{

    public override void ReceiveDamage(float dmg, GameObject source, bool eat=false)
    {
        if (eat)
        {
            source.GetComponent<Zombie>().Hypnotize();
            Die();
        }
        else base.ReceiveDamage(dmg, source, eat);
    }

}
