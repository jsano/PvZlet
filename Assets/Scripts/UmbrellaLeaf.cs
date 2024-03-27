using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrellaLeaf : Plant
{

    public GameObject shield;
    private Coroutine visualCoroutine;

    public override void Start()
    {
        base.Start();
        shield.transform.localScale = Tile.TILE_DISTANCE * 2.5f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bungee>() != null)
        {
            collision.GetComponent<Bungee>().Die();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("ZombieLobbedProjectile"))
        {
            Destroy(collision.gameObject);
        }
        else return;
        if (visualCoroutine != null) StopCoroutine(visualCoroutine);
        visualCoroutine = StartCoroutine(Visual());
    }

    private IEnumerator Visual()
    {
        shield.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        shield.GetComponent<SpriteRenderer>().enabled = false;
    }

}
