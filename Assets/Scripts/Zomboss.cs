using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zomboss : Zombie
{

    public AudioClip spawn;
    public AudioClip ball;
    public AudioClip RV;
    public AudioClip defeated;

    private int currentBuild;
    private int maxBuild = 1;
    private int maxSingularBuild = 1;
    private float period;
    private float interval = 3;
    private bool idle = true;

    // Update is called once per frame
    public override void Update()
    {
        if (LevelManager.status == LevelManager.Status.Intro) return;
        if (status != null) {
            if (status.name == "Butter") status = null;
            else status.walkMod = Mathf.Max(status.walkMod, 0.5f);
        }

        if (idle && !changingLanes) period += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
        if (period >= interval)
        {
            idle = false;
            period = 0;
            float decision = Random.Range(0, 1f); // 1 = spawn, 2 = move, 3 = bungee, 4 = rv
            if (decision < 0.5f) StartCoroutine(SpawnZombie()); // 50%
            else if (decision < 0.75) 
            {
                MoveToLane(Random.Range(1, ZombieSpawner.Instance.lanes + 1), 0); // 25%
                idle = true;
            }
            else if (decision < 0.9) StartCoroutine(SpawnBungees()); // 15%
            else StartCoroutine(ThrowRV()); // 10%
        }
    }

    private IEnumerator SpawnZombie()
    {
        //1,9,31
        yield return null;
    }

    private IEnumerator SpawnBungees()
    {
        yield return null;
    }

    private IEnumerator ThrowRV()
    {
        yield return null;
    }

    protected override void Spawn()
    {
        transform.position = new Vector3(Tile.COL_TO_WORLD[9] + Tile.TILE_DISTANCE.x * 2, Tile.tileObjects[row, 9].transform.position.y, 0);
        StartCoroutine(Spawn_Helper());
    }

    private IEnumerator Spawn_Helper()
    {
        while (transform.position.x > Tile.tileObjects[3, 9].transform.position.x)
        {
            transform.Translate(Vector3.left * Time.deltaTime * 4);
            yield return null;
        }
        SFX.Instance.Play(enterSFX);
        yield return new WaitForSeconds(0.5f);
        while (transform.position.x > Tile.tileObjects[3, 8].transform.position.x)
        {
            transform.Translate(Vector3.left * Time.deltaTime * 2);
            yield return null;
        }
        SFX.Instance.Play(enterSFX);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Plant>() != null) collision.GetComponent<Plant>().ReceiveDamage(damage, gameObject, disintegrating: true);
    }

    public override void Die()
    {
        if (BC.enabled) SFX.Instance.Play(defeated);
        BC.enabled = false;
        StartCoroutine(BreakDown());
    }

    private IEnumerator BreakDown()
    {
        while (SR.color.a > 0.5f)
        {
            SR.color -= new Color(0, 0, 0, 0.1f * Time.deltaTime);
            yield return null;
        }
    }

}
