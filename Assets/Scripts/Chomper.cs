using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chomper : Plant
{

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

    protected override void Attack(Zombie z)
    {
        z.ReceiveDamage(damage);
        chewPeriod = 0;
        SR.material.color -= Color.white / 2;
    }

}
