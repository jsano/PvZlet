using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeBean : Plant
{

    /// <summary> Wakes a plant, and then disappears </summary>
    protected override void Attack(Zombie z)
    {
        if (Tile.tileObjects[row, col].planted != null) Tile.tileObjects[row, col].planted.GetComponent<Plant>().Wake(); 
        Destroy(gameObject);
    }

}
