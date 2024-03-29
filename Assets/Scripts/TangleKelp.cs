using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangleKelp : Chomper
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
        base.Attack(z);
        Destroy(gameObject);
    }

}
