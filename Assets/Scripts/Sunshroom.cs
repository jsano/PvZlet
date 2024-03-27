using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunshroom : Sunflower
{

    public float grownAtkspd;
    public float growTime;
    private float growPeriod;
    private bool grown;

    public override void Start()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 1);
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (sky.night) growPeriod += Time.deltaTime;
        if (!grown && growPeriod > growTime)
        {
            grown = true;
            transform.localScale = Vector3.one;
            atkspd = grownAtkspd;
        }
        base.Update();
    }
}
