using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackInTheBox : Football
{

    public float maxTime;
    private float remaining;
    private bool exploded;
    public GameObject explosion;

    public AudioClip boingSFX;
    public AudioClip[] gaspSFX;
    public AudioClip explosionSFX;

    public override void Start()
    {
        base.Start();
        remaining = Random.Range(3, maxTime);
        projectile = Instantiate(projectile, transform, false);
        projectile.transform.localPosition = new Vector3(-transform.localScale.x / 2, 0, 0);
        projectile.GetComponent<SpriteRenderer>().sortingOrder = SR.sortingOrder + 2;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!exploded)
        {
            if (projectile != null) remaining -= Time.deltaTime;
            base.Update();
            if (remaining <= 0)
            {
                projectile = null;
                exploded = true;
                StartCoroutine(Explode());
            }
        }
    }

    private IEnumerator Explode()
    {
        RB.velocity = Vector3.zero;
        StopEating();
        BC.enabled = false;
        SFX.Instance.Play(boingSFX);
        yield return new WaitForSeconds(0.25f);
        SFX.Instance.Play(gaspSFX[Random.Range(0, gaspSFX.Length)]);
        yield return new WaitForSeconds(0.75f);
        SFX.Instance.Play(explosionSFX);
        Vector2 area = new Vector2(2.5f, 2.5f);
        GameObject g = Instantiate(explosion, transform.position, Quaternion.identity);
        g.transform.localScale = area * Tile.TILE_DISTANCE;
        int mask = hypnotized ? LayerMask.GetMask("Zombie") : LayerMask.GetMask("Plant");
        RaycastHit2D[] all = Physics2D.BoxCastAll(transform.position, area * Tile.TILE_DISTANCE, 0, Vector2.zero, 0, mask);
        foreach (RaycastHit2D a in all)
        {
            a.collider.GetComponent<Damagable>().ReceiveDamage(1000, gameObject);
        }
        Die();
    }

}
