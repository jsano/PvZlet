using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hypnoshroom : Plant
{

    public AudioClip hypnotizeStart;

    public override float ReceiveDamage(float dmg, GameObject source, bool eat = false, bool disintegrating = false)
    {
        if (eat)
        {
            SFX.Instance.Play(hypnotizeStart);
            source.GetComponent<Zombie>().Hypnotize();
            Die();
            return 0;
        }
        else return base.ReceiveDamage(dmg, source, eat, disintegrating);
    }

}
