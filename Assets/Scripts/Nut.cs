using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nut : Plant
{

    private Sprite normalSprite;
    public Sprite damagedSprite1;
    public Sprite damagedSprite2;

    public override void Start()
    {
        normalSprite = SR.sprite;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (HP / baseHP > 2f/3) SR.sprite = normalSprite;
        else if (HP / baseHP > 1f/3) SR.sprite = damagedSprite1;
        else SR.sprite = damagedSprite2;
        base.Update();
    }

}
