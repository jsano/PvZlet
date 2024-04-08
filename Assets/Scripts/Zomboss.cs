using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Zomboss : Zombie
{

    public AudioClip spawn;
    public AudioClip ball;
    public AudioClip RV;
    public AudioClip defeated;

    private int currentBuild;
    private int maxBuild = 4;
    private int maxSingularBuild = 2;
    private float period;
    private float interval = 3;
    private bool idle = true;

    // Update is called once per frame
    public override void Update()
    {
        if (LevelManager.status == LevelManager.Status.Intro) return;
        if (status != null) {
            status.walkMod = Mathf.Max(status.walkMod, 0.5f);
        }

        if (idle && !changingLanes) period += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
        if (period >= interval)
        {
            idle = false;
            period = 0;
            if (currentBuild >= maxBuild)
            {
                StartCoroutine(MakeBall());
                maxBuild += 4;
                maxSingularBuild += 2;
                currentBuild = 0;
            }
            else
            {
                float decision = Random.Range(0, 1f); // 1 = spawn, 2 = move, 3 = bungee, 4 = rv
                if (decision < 0.4f) StartCoroutine(SpawnZombie()); // 40%
                else if (decision < 0.8f) 
                {
                    MoveToLane(Random.Range(1, ZombieSpawner.Instance.lanes + 1), 0); // 40%
                    idle = true;
                }
                else if (decision < 0.9f) StartCoroutine(SpawnBungees()); // 10%
                else StartCoroutine(ThrowRV()); // 10%
            }
        }
    }

    private IEnumerator SpawnZombie()
    {
        List<int> possible = new List<int>();
        for (int i = 0; i < ZombieSpawner.Instance.allZombies.Length; i++)
        {
            if (i == 1 || i == 9 || i == 22 || i == 31) continue; // Flag, Backup, Bungee, Zomboss
            Zombie temp = ZombieSpawner.Instance.allZombies[i].GetComponent<Zombie>();
            if (temp.aquatic) continue;
            if (temp.spawnScore > maxSingularBuild) continue;
            if (i == 15 && Tile.tileObjects[row, 7].ContainsGridItem("Snow")) continue;
            possible.Add(i);
        }
        SFX.Instance.Play(spawn);
        GameObject g = ZombieSpawner.Instance.allZombies[possible[Random.Range(0, possible.Count)]];
        Zombie z = Instantiate(g, Tile.tileObjects[row, 7].transform.position, Quaternion.identity).GetComponent<Zombie>();
        z.row = row;
        currentBuild += z.spawnScore;
        /*Vector3 to = z.transform.localScale;
        z.transform.localScale = Vector3.zero;
        float frame = 0;
        while (z.transform.localScale.magnitude < to.magnitude)
        {
            z.transform.localScale = Vector3.Lerp(Vector3.zero, to, frame);
            frame += Time.deltaTime;
        }*/
        yield return null;
        idle = true;
    }

    private IEnumerator SpawnBungees()
    {
        GameObject[] b = new GameObject[maxBuild / 5];
        for (int i = 0; i < maxBuild / 5; i++) b[i] = Instantiate(ZombieSpawner.Instance.allZombies[22]);
        yield return new WaitUntil(() => b.Count(g => g != null) == 0);
        idle = true;
    }

    private IEnumerator ThrowRV()
    {
        yield return null;
        idle = true;
    }

    private IEnumerator MakeBall()
    {
        yield return null;
        idle = true;
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
