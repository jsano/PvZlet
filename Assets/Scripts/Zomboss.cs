using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Zomboss : Zombie
{

    public GameObject[] balls;
    public GameObject RV;
    public AudioClip spawn;
    public AudioClip move;
    public AudioClip ball;
    public AudioClip throwRV;
    public AudioClip defeated;

    private int currentCount;
    private int maxCount = 6;
    private int maxSingularBuild = 2;
    private int minSingularBuild = -1;
    private float period;
    private float interval = 5;
    private bool threwRV;
    private bool bungeed;
    private bool idle = true;

    private int sortingOrder;

    // Update is called once per frame
    public override void Update()
    {
        if (LevelManager.status != LevelManager.Status.Start) return;
        if (status != null) {
            status.walkMod = Mathf.Max(status.walkMod, 0.5f);
        }

        if (!idle) transform.rotation = Quaternion.Euler(0, 0, 30);
        else transform.rotation = Quaternion.identity;
        if (idle && !changingLanes) period += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
        if (period >= interval)
        {
            idle = false;
            period = 0;
            if (currentCount >= maxCount)
            {
                StartCoroutine(MakeBall());
                maxCount += 1;
                maxSingularBuild += 2;
                minSingularBuild = Mathf.Min(minSingularBuild + 1, 4);
                currentCount = 0;
                interval = Mathf.Max(interval - 0.25f, 2);
                threwRV = false;
                bungeed = false;
            }
            else
            {
                float decision = Random.Range(0, 1f);
                if (decision < 0.45f) StartCoroutine(SpawnZombie()); // 45%
                else if (decision < 0.8f) // 35%
                {
                    SFX.Instance.Play(move);
                    int newLane;
                    do
                    {
                        newLane = Random.Range(1, ZombieSpawner.Instance.lanes + 1);
                    } while (newLane == row);
                    MoveToLane(newLane, 0);
                    idle = true;
                }
                else if (decision < 0.9f) // 10%
                {
                    if (!bungeed && maxCount >= 8) StartCoroutine(SpawnBungees());
                    else StartCoroutine(SpawnZombie());
                }
                else // 10%
                {
                    if (!threwRV && maxCount >= 10) StartCoroutine(ThrowRV());
                    else StartCoroutine(SpawnZombie());
                }
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
            if (temp.spawnScore > maxSingularBuild || temp.spawnScore < minSingularBuild) continue;
            if (i == 15 && !Tile.tileObjects[row, 7].ContainsGridItem("Snow")) continue;
            possible.Add(i);
        }
        SFX.Instance.Play(spawn);
        GameObject g = ZombieSpawner.Instance.allZombies[possible[Random.Range(0, possible.Count)]];
        Zombie z = Instantiate(g, Tile.tileObjects[row, 7].transform.position, Quaternion.identity).GetComponent<Zombie>();
        z.row = row;
        z.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
        sortingOrder += 1;
        currentCount += 1;
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
        Bungee[] b = new Bungee[maxCount / 4];
        for (int i = 0; i < maxCount / 4; i++)
        {
            Bungee cur = Instantiate(ZombieSpawner.Instance.allZombies[22]).GetComponent<Bungee>();
            cur.row = 0;
            cur.col = 0;
            b[i] = cur;
        }
        yield return new WaitUntil(() => b.Count(g => g != null) == 0);
        idle = true;
        bungeed = true;
    }

    private IEnumerator ThrowRV()
    {
        int r = Random.Range(1, ZombieSpawner.Instance.lanes);
        int c = Random.Range(2, 5);
        GameObject g = Instantiate(RV, Tile.tileObjects[r, c].transform.position + new Vector3(0, Tile.TILE_DISTANCE.y * 7), Quaternion.Euler(0, 0, 7.5f));
        Rigidbody2D rb = g.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, -1);
        yield return new WaitUntil(() => g.transform.position.y <= Tile.tileObjects[r, c].transform.position.y);
        SFX.Instance.Play(throwRV);
        for (int i = r; i <= r + 1; i++) for (int j = c - 1; j <= c + 1; j++) Tile.tileObjects[i, j].RemoveAllPlants();
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(-10, 10), ForceMode2D.Impulse);
        idle = true;
        threwRV = true;
        yield return new WaitUntil(() => g.transform.position.y <= -15);
        Destroy(g);
    }

    private IEnumerator MakeBall()
    {
        SFX.Instance.Play(ball);
        GameObject g = Instantiate(balls[Random.Range(0, balls.Length)], Tile.tileObjects[row, 7].transform.position - new Vector3(0, Tile.TILE_DISTANCE.y / 2), Quaternion.identity);
        g.GetComponentInChildren<ZombotBall>().row = row;
        yield return new WaitUntil(() => g.transform.GetChild(0).rotation.eulerAngles.z != 0);
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
        if (BC.enabled)
        {
            Zombie[] left = FindObjectsByType<Zombie>(FindObjectsSortMode.None);
            foreach (Zombie zombie in left) if (zombie != this) zombie.Die();
            SFX.Instance.Play(defeated);
            StartCoroutine(BreakDown());
        }
        BC.enabled = false;
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
