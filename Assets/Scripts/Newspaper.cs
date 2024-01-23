using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Newspaper : Zombie
{

    public float angryWalkTime;
    public float angryEatTime;
    private bool angry = false;
    private bool shocked = false;

    // Update is called once per frame
    public override void Update()
    {
        if (shield != null || angry) base.Update();
        else if (!shocked)
        {
            shocked = true;
            StopEating();
            StartCoroutine(Shock());
            walkTime = angryWalkTime;
            eatTime = angryEatTime;
        }
    }

    private IEnumerator Shock()
    {
        yield return new WaitForSeconds(1.5f);
        angry = true;
    }

}
