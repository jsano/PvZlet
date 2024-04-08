using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zomboss : Zombie
{

    // Update is called once per frame
    public override void Update()
    {
        
    }

    protected override void Spawn()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Plant>() != null) collision.GetComponent<Plant>().ReceiveDamage(damage, gameObject, disintegrating: true);
    }

    public override void Die()
    {
        BC.enabled = false;
    }

}
