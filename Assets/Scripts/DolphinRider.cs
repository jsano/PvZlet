using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinRider : PoleVaulter
{

    private bool entered;

    public override void Start()
    {
        base.Start();
        BC.offset = new Vector2(0, -0.5f);
        BC.size = new Vector2(1, 0.01f);
        StartCoroutine(EnterPool());
    }

    // Update is called once per frame
    public override void Update()
    {
        if (entered)
        {
            base.Update();
        }
        else
        {
            RB.velocity = new Vector2(-0.5f, 0);
        }
    }

    private IEnumerator EnterPool()
    {
        yield return new WaitForSeconds(3);
        entered = true;
        BC.offset = new Vector2(0, 0);
        BC.size = new Vector2(1, 1);
    }

}
