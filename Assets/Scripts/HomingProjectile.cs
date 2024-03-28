using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : StraightProjectile
{

    [HideInInspector] public GameObject toChase;

    // Update is called once per frame
    public override void Update()
    {
        if (toChase != null)
        {
            RB.velocity += (Vector2)(toChase.transform.position - transform.position) * 8 * Time.deltaTime;
            RB.velocity = RB.velocity.normalized * speed;
            Vector2 dir = toChase.transform.position - transform.position;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * 180 / Mathf.PI);
        }
        base.Update();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != toChase && other.transform.parent != toChase) return;
        base.OnTriggerEnter2D(other);
    }

}
