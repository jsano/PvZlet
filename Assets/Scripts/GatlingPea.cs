using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingPea : Peashooter
{

    /// <summary> Shoots 4 peas quickly </summary>
    protected override void Attack(Zombie z)
    {
        StartCoroutine(Attack_Helper(z));
    }

    private IEnumerator Attack_Helper(Zombie z)
    {
        base.Attack(z);
        yield return new WaitForSeconds(0.2f);
        SFX.Instance.Play(Random.Range(0, 1f) < 0.5f ? attackSFX2 : attackSFX1);
        base.Attack(z);
        yield return new WaitForSeconds(0.2f);
        SFX.Instance.Play(Random.Range(0, 1f) < 0.5f ? attackSFX2 : attackSFX1);
        base.Attack(z);
        yield return new WaitForSeconds(0.2f);
        SFX.Instance.Play(Random.Range(0, 1f) < 0.5f ? attackSFX2 : attackSFX1);
        base.Attack(z);
    }

}
