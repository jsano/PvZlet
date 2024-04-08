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
        if (collision.GetComponent<ZombotBall>() != null) return;
        Zombie z = collision.GetComponent<Zombie>();
        if (z != null && LevelManager.status != LevelManager.Status.Lost)
        {
            levelManager.Lose();
            collision.GetComponent<SpriteRenderer>().sortingLayerName = "Sun";
            collision.GetComponent<SpriteRenderer>().sortingOrder = 2;
            if (z.armor != null)
            {
                z.armor.GetComponent<SpriteRenderer>().sortingLayerName = "Sun";
                z.armor.GetComponent<SpriteRenderer>().sortingOrder = 3;
            }
            if (z.shield != null)
            {
                z.shield.GetComponent<SpriteRenderer>().sortingLayerName = "Sun";
                z.armor.GetComponent<SpriteRenderer>().sortingOrder = 4;
            }
            // TODO: handle projectile without overwriting prefabs (ex catapult)
        }
    }

    public override float ReceiveDamage(float dmg, GameObject source, bool eat = false, bool disintegrating = false)
    {
        return 1;
    }

}
