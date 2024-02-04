using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : Damagable
{

    public static bool lost;
    public GameObject gameOver;

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Zombie>() != null && !lost)
        {
            lost = true;
            gameOver.SetActive(true);
            GameObject.Find("UI").SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            collision.GetComponent<SpriteRenderer>().sortingLayerName = "Sun";
            collision.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
    }

    public override void ReceiveDamage(float dmg, GameObject source, bool eat = false)
    {
        return;
    }

}
