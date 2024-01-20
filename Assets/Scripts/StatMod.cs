using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatMod : ScriptableObject
{

    private class Data
    {
        public float walkMod;
        public float eatMod;
        public Color c;
    }

    private string statusName;

    private static Dictionary<string, Data> effects = new Dictionary<string, Data>()
    {
        {"chill", new Data{ walkMod = 0.5f, eatMod = 0.5f, c = new Color(0, 0.5f, 1, 1) } },
    };

    private Zombie target;

    public void Apply(Zombie z, string name)
    {
        statusName = name;
        target = z;
        target.walkTime *= 1 / effects[statusName].walkMod;
        target.eatTime *= 1 / effects[statusName].eatMod;
        target.StartCoroutine(Unapplication());
    }

    public IEnumerator Unapplication()
    {
        yield return new WaitForSeconds(10);
        Remove();
    }

    public void Remove()
    {
        if (target == null) return;
        target.walkTime /= 1 / effects[statusName].walkMod;
        target.eatTime /= 1 / effects[statusName].eatMod;
        target.status = null;
        target = null;
    }

    public Color GetColor()
    {
        return effects[statusName].c;
    }

}
