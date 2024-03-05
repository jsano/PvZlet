using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snorkel : Zombie
{

    // Update is called once per frame
    public override void Update()
    {
        int c = Mathf.Max(Tile.WORLD_TO_COL(transform.position.x), 1);
        if (c < 10 && Tile.tileObjects[row, c].water && !isEating()) Submerge();
        else Unsubmerge();
        base.Update();
    }

    protected override IEnumerator Eat(Damagable p)
    {
        Unsubmerge();
        while (true) yield return base.Eat(p);
    }

    private void Submerge()
    {
        BC.offset = new Vector2(0, -0.5f);
        BC.size = new Vector2(1, 0.01f);
    }

    private void Unsubmerge()
    {
        BC.offset = new Vector2(0, 0);
        BC.size = new Vector2(1, 1);
    }

}
