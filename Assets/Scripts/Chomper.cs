using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chomper : Plant
{

    /// <summary> How long in seconds it takes to finish chewing a zombie. Will not call the base <c>Update</c> until it's finished chewing </summary>
    public float chewTime;
    private float chewPeriod;

    private Sprite normalSprite;
    public Sprite chewSprite;

    public override void Start()
    {
        chewPeriod = chewTime;
        normalSprite = SR.sprite;
        base.Start();
    }

    public override void Update()
    {
        chewPeriod += Time.deltaTime;
        if (chewPeriod > chewTime)
        {
            SR.sprite = normalSprite;
            base.Update();
        }
    }

    /// <summary> Eats a zombie, and then starts chewing </summary>
    protected override void Attack(Zombie z)
    {
        z.ReceiveDamage(damage, null);
        chewPeriod = 0;
        SR.sprite = chewSprite;
        base.Attack(z);
    }

}
