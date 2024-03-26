using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DolphinRider : PoleVaulter
{

    private bool entered;

    public override void Start()
    {
        base.Start();
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
        yield return new WaitForSeconds(2);
        gameObject.layer = LayerMask.NameToLayer("Zombie");
        yield return new WaitForSeconds(1);
        entered = true;
    }

}
