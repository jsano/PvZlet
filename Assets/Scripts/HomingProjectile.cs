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
            // NOTE: Probably wrong
            transform.rotation = Quaternion.Euler(Vector3.RotateTowards(transform.forward, toChase.transform.position - transform.position, Time.deltaTime, 0));
        }
        base.Update();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != toChase && other.transform.parent != toChase) return;
        base.OnTriggerEnter2D(other);
    }

}
