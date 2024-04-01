using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargantuar : Zombie
{

    public GameObject imp;
    private bool hasImp = true;
    private bool attacking;
    private bool throwing;
    private bool dead;

    public AudioClip windup;
    public AudioClip smash;
    public AudioClip death;
    public AudioClip[] impThrow;

    // Update is called once per frame
    public override void Update()
    {
        if (dead) return;
        if (!attacking && hasImp && HP <= baseHP / 3 && Tile.WORLD_TO_COL(transform.position.x) >= 5)
        {
            hasImp = false;
            ResetWalk();
            StartCoroutine(Throw());
        }
        if (!attacking && !throwing) base.Update();
    }

    protected override void Eat(GameObject p)
    {
        if (!attacking)
        {
            attacking = true;
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack() 
    {
        ResetWalk();
        SFX.Instance.Play(windup);
        float period = 0;
        while (period < 1.5f)
        {
            period += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
            yield return null;
        }
        SFX.Instance.Play(smash);
        int c = Mathf.Clamp(Tile.WORLD_TO_COL(transform.position.x + Tile.TILE_DISTANCE.x / 3), 1, 9);
        if (Tile.tileObjects[row, c].planted != null) Tile.tileObjects[row, c].RemoveAllPlants(gameObject);
        else
        {
            c = Mathf.Clamp(Tile.WORLD_TO_COL(transform.position.x - Tile.TILE_DISTANCE.x / 3), 1, 9);
            Tile.tileObjects[row, c].RemoveAllPlants(gameObject);
        }
        period = 0;
        while (period < 0.5f)
        {
            period += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
            yield return null;
        }
        attacking = false;
    }

    private IEnumerator Throw()
    {
        throwing = true;
        float period = 0;
        while (period < 0.75f)
        {
            if (dead) yield break;
            period += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
            yield return null;
        }
        GameObject g = Instantiate(imp, transform.position + new Vector3(0, Tile.TILE_DISTANCE.y, 0), Quaternion.identity);
        g.GetComponent<Imp>().flung = true;
        g.GetComponent<Imp>().row = row;
        throwing = false;
        SFX.Instance.Play(impThrow[Random.Range(0, impThrow.Length)]);
    }

    public override void Die()
    {
        if (!dead)
        {
            dead = true;
            BC.enabled = false;
            RB.velocity = Vector2.zero;
            ZombieSpawner.Instance.SubtractBuild(spawnScore, waveNumber);
            SFX.Instance.Play(death);
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {
        float period = 0;
        while (period < 4f)
        {
            period += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
            yield return null;
        }
        SFX.Instance.Play(smash);
        Destroy(gameObject);
    }

}
