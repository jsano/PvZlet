using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garlic : Nut
{

    public AudioClip[] yuckSFX;

    public override float ReceiveDamage(float dmg, GameObject source, bool eat = false, bool disintegrating = false)
    {
        if (eat)
        {
            Zombie z = source.GetComponent<Zombie>();
            int newRow;
            if (z.row == ZombieSpawner.Instance.lanes) newRow = z.row - 1;
            else if (z.row == 1) newRow = z.row + 1;
            else newRow = z.row + 1 - 2 * Random.Range(0, 2);
            z.MoveToLane(newRow, 1.5f);
            StartCoroutine(Yuck());
        }
        return base.ReceiveDamage(dmg, source, eat, disintegrating);
    }

    private IEnumerator Yuck()
    {
        yield return new WaitForSeconds(0.5f);
        SFX.Instance.Play(yuckSFX[Random.Range(0, yuckSFX.Length)]);
    }

}
