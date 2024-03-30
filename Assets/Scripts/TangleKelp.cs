using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangleKelp : Plant
{

    private bool attacked;

    public AudioClip caught;

    protected override void Attack(Zombie z)
    {
        if (!attacked)
        {
            SFX.Instance.Play(caught);
            attacked = true;
            z.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(Wait(z));
        }
    }

    private IEnumerator Wait(Zombie z)
    {
        yield return new WaitForSeconds(1f);
        z.ReceiveDamage(damage, null, disintegrating: true);
        base.Attack(z);
        Destroy(gameObject);
    }

}
