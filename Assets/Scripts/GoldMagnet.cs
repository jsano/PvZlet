using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoldMagnet : Plant
{

    protected override void Attack(Zombie z)
    {
        Sun[] s = FindObjectsByType<Sun>(FindObjectsSortMode.None);
        List<Sun> suns = s[0..Mathf.Min(5, s.Length)].ToList<Sun>();
        StartCoroutine(Attack_Helper(suns));
    }

    private IEnumerator Attack_Helper(List<Sun> suns)
    {
        int count = 1;
        while (count > 0)
        {
            count = 0;
            foreach (Sun s in suns)
            {
                if (s == null) continue;
                count += 1;
                s.transform.Translate((transform.position - s.transform.position).normalized * 15 * Time.deltaTime);
                if (Vector3.Distance(s.transform.position, transform.position) < 0.1f) s.Collect();
            }
            yield return null;
        }
        base.Attack(null);
    }

}
