using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikerock : Spikeweed
{

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

}
