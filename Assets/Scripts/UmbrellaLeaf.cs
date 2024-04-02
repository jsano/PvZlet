using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrellaLeaf : Plant
{

    public GameObject shield;
    private Coroutine visualCoroutine;

    public AudioClip boing;

    public override void Start()
    {
        base.Start();
        shield.transform.localScale = Tile.TILE_DISTANCE * 3f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bungee>() != null || collision.GetComponent<Imp>() != null && collision.GetComponent<Imp>().flung)
        {
            if (Mathf.Abs(collision.GetComponent<Zombie>().row - row) > 1) return;
            SFX.Instance.Play(boing);
            collision.GetComponent<Zombie>().Die();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("ZombieLobbedProjectile"))
        {
            if (Mathf.Abs(collision.GetComponent<LobbedProjectile>().lane - row) > 1) return;
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
