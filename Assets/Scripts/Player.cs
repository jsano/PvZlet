using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        if (collision.GetComponent<Zombie>() != null && LevelManager.status != LevelManager.Status.Lost)
        {
            levelManager.Lose();
            collision.GetComponent<SpriteRenderer>().sortingLayerName = "Sun";
            collision.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
    }

    public override void ReceiveDamage(float dmg, GameObject source, bool eat = false)
    {
        return;
    }

}
