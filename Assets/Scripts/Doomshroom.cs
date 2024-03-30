using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doomshroom : CherryBomb
{
    
    // NOTE: will use DestroyAfterAnimation for now but will probably need its own script since it's not an "animation"
    public GameObject crater;

    protected override void Attack(Zombie z)
    {
        Tile.tileObjects[row, col].Place(crater);
        base.Attack(z);
    }

}
