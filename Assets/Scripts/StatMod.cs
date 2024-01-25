using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatMod : ScriptableObject
{

    /// <summary> Object to store differing data types for dictionary value purposes </summary>
    public class Data
    {
        public float duration;
        public float walkMod;
        public float eatMod;
        public string removedBy;
        public Color c;
    }

    /// <summary> Zombies will move at their normal speed <c>* walkMod</c> </summary>
    public float walkMod;
    /// <summary> Zombies will eat at their normal speed <c>* eatMod</c> </summary>
    public float eatMod;
    /// <summary> If zombies are hit by this condition, it removes the effect </summary>
    public string removedBy;
    /// <summary> Zombies will be tinted this color </summary>
    public Color colorTint;

    /// <summary> The global mapping of all status effects in the game, labeled by name </summary>
    private static Dictionary<string, Data> effects = new Dictionary<string, Data>()
    {
        {"Chill", new Data{ duration = 10, walkMod = 0.5f, eatMod = 0.5f, removedBy = "Fire", c = new Color(0, 0.5f, 1, 1) } },
        {"Freeze", new Data{ duration = 6, walkMod = 0, eatMod = 0, removedBy = "Fire", c = new Color(0, 0.5f, 1, 1) } },
    };

    /// <summary> The zombie this effect is applied to. Can be null if this effect is overwritten or the zombie dies </summary>
    private Zombie target;

    /// <summary> Required function for the status effect to take place. Applies the modifiers and removes itself after time </summary>
    /// <param name="z"> The zombie to apply the effect to </param>
    /// <param name="name"> The name of the status effect. Refer to <c>StatMod.effects</c> </param>
    public void Apply(Zombie z, string name)
    {
        if (z.GetComponent<Zomboni>() != null) return; //NOTE: Maybe make "status effectable" into a field
        target = z;
        if (target.status) target.status.Remove();
        target.status = this;
        walkMod = effects[name].walkMod;
        eatMod = effects[name].eatMod;
        removedBy = effects[name].removedBy;
        colorTint = effects[name].c;
        target.StartCoroutine(Unapplication(effects[name].duration));
    }

    private IEnumerator Unapplication(float dur)
    {
        yield return new WaitForSeconds(dur);
        Remove();
    }

    /// <summary> Removes the effect from the target zombie by unlinking all references to each other. If already unlinked, does nothing </summary>
    public void Remove()
    {
        if (target == null) return;
        target.status = null;
        target.getSpriteRenderer().material.color = target.getBaseColor();
        target = null;
    }

}
