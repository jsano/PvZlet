using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinRider : PoleVaulter
{

    private bool entered;

    public override void Start()
    {
        base.Start();
        Destroy(projectile);
        projectile = Instantiate(projectile, transform, false);
        projectile.transform.localPosition = new Vector3(0, -BC.size.y / 2, 0);
        projectile.GetComponent<SpriteRenderer>().sortingOrder = SR.sortingOrder + 2;
    }

    protected override void Spawn()
    {
        base.Spawn();
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
        yield return new WaitForSeconds(2.5f);
        gameObject.layer = LayerMask.NameToLayer("Zombie");
        yield return new WaitForSeconds(0.5f);
        entered = true;
    }

}
