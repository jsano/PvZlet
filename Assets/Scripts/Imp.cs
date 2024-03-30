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
            RB.gravityScale = 1f;
            gameObject.layer = LayerMask.NameToLayer("ExplosivesOnly");
            RB.velocity = Vector2.left * 8f;
            landed = false;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!landed && transform.position.y <= Tile.tileObjects[row, Mathf.Min(9, Mathf.Max(1, Tile.WORLD_TO_COL(transform.position.x)))].transform.position.y)
        {
            gameObject.layer = LayerMask.NameToLayer("Zombie");
            RB.gravityScale = 0;
            RB.velocity = Vector2.zero;
            landed = true;
        }
        if (landed) base.Update();
    }

}
