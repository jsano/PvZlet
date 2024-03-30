using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Newspaper : Zombie
{

    public float angryWalkTime;
    public float angryEatTime;
    private bool angry = false;
    private bool shocked = false;

    public AudioClip ripSFX;
    public AudioClip[] angrySFX;

    // Update is called once per frame
    public override void Update()
    {
        if (shield != null || angry) base.Update();
        else if (!shocked)
        {
            SFX.Instance.Play(ripSFX);
            shocked = true;
            StartCoroutine(Shock());
            walkTime = angryWalkTime;
            eatTime = angryEatTime;
        }
    }

    private IEnumerator Shock()
    {
        StopEating();
        ResetWalk();
        float wait = 0;
        while (wait < 1.5f)
        {
            wait += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
            yield return null;
        }
        SFX.Instance.Play(angrySFX[Random.Range(0, angrySFX.Length)]);
        angry = true;
    }

}
