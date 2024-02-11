using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chomper : Plant
{

    /// <summary> How long in seconds it takes to finish chewing a zombie. Will not call the base <c>Update</c> until it's finished chewing </summary>
    public float chewTime;
    private float chewPeriod;

    public override void Start()
    {
        chewPeriod = chewTime;
        base.Start();
    }

    public override void Update()
    {
        chewPeriod += Time.deltaTime;
        if (chewPeriod > chewTime)
        {
            SR.material.color = Color.white;
            base.Update();
        }
    }

    /// <summary> Eats a zombie, and then starts chewing </summary>
    protected override void Attack(Zombie z)
    {
        z.ReceiveDamage(damage, null);
        chewPeriod = 0;
        SR.material.color -= Color.white / 2; // will cause jank but it's ok since this will be replaced with animations
        base.Attack(z);
    }

}
