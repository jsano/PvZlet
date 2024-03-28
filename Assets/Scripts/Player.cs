using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Damagable
{

    public LevelManager levelManager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Zombie z = collision.GetComponent<Zombie>();
        if (z != null && LevelManager.status != LevelManager.Status.Lost)
        {
            levelManager.Lose();
            collision.GetComponent<SpriteRenderer>().sortingLayerName = "Sun";
            collision.GetComponent<SpriteRenderer>().sortingOrder = 2;
            if (z.armor != null) z.armor.GetComponent<SpriteRenderer>().sortingLayerName = "Sun";
            if (z.shield != null) z.shield.GetComponent<SpriteRenderer>().sortingLayerName = "Sun";
            // TODO: handle projectile without overwriting prefabs (ex catapult)
        }
    }

    public override void ReceiveDamage(float dmg, GameObject source, bool eat = false)
    {
        return;
    }

}
