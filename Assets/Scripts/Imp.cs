using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : Zombie
{

    [HideInInspector] public bool flung;
    private bool landed = true;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        if (flung)
        {
            RB.gravityScale = 1.5f;
            BC.enabled = false;
            RB.velocity = Vector2.left * 13f;
            landed = false;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!landed && transform.position.y <= Tile.tileObjects[row, Mathf.Min(9, Tile.WORLD_TO_COL(transform.position.x))].transform.position.y)
        {
            BC.enabled = true;
            RB.gravityScale = 0;
            RB.velocity = Vector2.zero;
            landed = true;
        }
        if (landed) base.Update();
    }

}
