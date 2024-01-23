using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunshroom : Sunflower
{

    public float grownAtkspd;
    public float growTime;
    private float growPeriod;
    private bool grown;

    // Update is called once per frame
    public override void Update()
    {
        if (sky.night) growPeriod += Time.deltaTime;
        if (!grown && growPeriod > growTime)
        {
            grown = true;
            atkspd = grownAtkspd;
        }
        base.Update();
    }
}
