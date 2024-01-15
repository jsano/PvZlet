using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    public int spawnScore;
    public int damage;
    public float walkTime; // Seconds per tile
    private float period = 0;
    public float HP;
    [HideInInspector] public int row = 1; // [1-5]

    public GameObject projectile;

    private Rigidbody2D RB;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        if (HP <= 0)
        {
            Die();
        }
    }

    protected void Spawn()
    {
        transform.position = new Vector3(Tile.TILE_DISTANCE.x * 7, Tile.TILE_DISTANCE.y * (3 - row), 0);
    }

    protected void Walk()
    {
        period += Time.deltaTime;
        if (period >= walkTime / 2)
        {
            period = 0;
            StartCoroutine(Walk_Helper());
        }
    }

    private IEnumerator Walk_Helper()
    {
        RB.velocity = new Vector2(-Tile.TILE_DISTANCE.x / 2 / 0.5f, 0); // d = rt
        yield return new WaitForSeconds(0.5f);
        RB.velocity = Vector3.zero;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Plant>() != null)
        {
            RB.velocity = Vector2.zero;
            StartCoroutine(Eat(collision.GetComponent<Plant>()));
        }
        
    }

    protected IEnumerator Eat(Plant p)
    {
        while (p != null)
        {
            period = 0;
            p.ReceiveDamage(damage);
            yield return new WaitForSeconds(0.5f);
        }
        
    }

    public virtual void ReceiveDamage(float dmg)
    {
        HP -= dmg;
        StartCoroutine(HitVisual());
    }

    protected IEnumerator HitVisual()
    {
        GetComponent<SpriteRenderer>().color -= Color.blue / 2;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color += Color.blue / 2;
    }

    protected void Die()
    {
        GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>().currentBuild -= spawnScore;
        Destroy(gameObject);
    }

}
