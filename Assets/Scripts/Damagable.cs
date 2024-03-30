using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damagable : MonoBehaviour
{

    /// <summary> Called when something deals damage to this </summary>
    /// <param name="dmg"> How much damage to deal </param>
    /// <param name="source"> The source object that dealt the damage. Only useful for eating zombies and can be null if unimportant </param>
    /// <param name="eat"> Whether this was eating damage </param>
    /// <returns> The remaining HP this has after the interaction </returns>
    public abstract float ReceiveDamage(float dmg, GameObject source, bool eat=false, bool disintegrating=false);

}
