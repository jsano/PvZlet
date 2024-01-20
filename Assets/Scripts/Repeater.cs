using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : Plant
{

    protected override void Attack(Zombie z)
    {
        StartCoroutine(Attack_Helper());
    }

    private IEnumerator Attack_Helper()
    {
        Instantiate(projectile, transform.position + rightOffset, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Instantiate(projectile, transform.position + rightOffset, Quaternion.identity);
    }

}
