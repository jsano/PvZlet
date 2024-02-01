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
        shield.transform.localScale = Tile.TILE_DISTANCE * 3;
    }

    // Update is called once per frame
    public override void Update()
    {
        for (int i = Mathf.Max(1, row - 2); i <= Mathf.Min(row + 2, ZS.lanes); i++)
            for (int j = Mathf.Max(1, col - 2); j <= Mathf.Min(col + 2, 9); j++)
            {
                if (Tile.tileObjects[i, j].fog != null)
                {
                    SpriteRenderer sr = Tile.tileObjects[i, j].fog.GetComponent<SpriteRenderer>();
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
                }
            }
        base.Update();
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
