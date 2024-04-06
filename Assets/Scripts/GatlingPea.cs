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
        Shoot();
        yield return new WaitForSeconds(0.2f);
        SFX.Instance.Play(attackSFX[Random.Range(0, attackSFX.Length)]);
        Shoot();
        yield return new WaitForSeconds(0.2f);
        SFX.Instance.Play(attackSFX[Random.Range(0, attackSFX.Length)]);
        Shoot();
        yield return new WaitForSeconds(0.2f);
        SFX.Instance.Play(attackSFX[Random.Range(0, attackSFX.Length)]);
        base.Attack(z);
    }

}
