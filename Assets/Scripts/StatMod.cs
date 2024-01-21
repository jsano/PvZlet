using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatMod : ScriptableObject
{

    public class Data
    {
        public float walkMod;
        public float eatMod;
        public Color c;
    }

    public float walkMod;
    public float eatMod;
    public Color c;

    private static Dictionary<string, Data> effects = new Dictionary<string, Data>()
    {
        {"chill", new Data{ walkMod = 0.5f, eatMod = 0.5f, c = new Color(0, 0.5f, 1, 1) } },
    };

    private Zombie target;

    public void Apply(Zombie z, string name)
    {
        target = z;
        if (target.status) target.status.Remove();
        target.status = this;
        walkMod = effects[name].walkMod;
        eatMod = effects[name].eatMod;
        c = effects[name].c;
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
        target.status = null;
        target.getSpriteRenderer().material.color = Color.white;
        target = null;
    }

}
