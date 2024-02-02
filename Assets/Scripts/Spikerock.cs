using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikerock : Spikeweed
{

    public int popsRemaining;

    protected override void Attack(Zombie z)
    {
        StartCoroutine(Attack_Helper(z));
    }

    private IEnumerator Attack_Helper(Zombie z)
    {
        base.Attack(z);
        yield return new WaitForSeconds(0.2f);
        base.Attack(z);
    }

    public override void ReceiveDamage(float dmg, GameObject source, bool eat = false)
    {
        if (source.GetComponent<Zombie>() != null && (source.GetComponent<Zombie>().wheels || source.GetComponent<Gargantuar>() != null))
        {
            if (source.GetComponent<Zombie>().wheels) source.GetComponent<Zombie>().ReceiveDamage(1000, gameObject);
            popsRemaining -= 1;
            if (popsRemaining <= 0) Die();
        }
        else base.ReceiveDamage(dmg, source, eat);
    }

}
