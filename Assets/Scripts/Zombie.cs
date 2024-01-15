using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    public float spawnScore;
    public int damage;
    public float walkTime; // Seconds per tile
    private float period = 0;
    private bool walking = false;
    private float walkPeriod = 0;
    public int HP;
    [HideInInspector] public int row = 1; // [1-5]

    public GameObject projectile;

    private Rigidbody2D RB;

    // Start is called before the first frame update
    void Start()
    {
        row = Random.Range(1, 5);
        RB = GetComponent<Rigidbody2D>();
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Tile.TILE_DISTANCE.x / 5, LayerMask.GetMask("Plant"));
        //if (hit) StartCoroutine(Eat(hit.collider.GetComponent<Plant>()));
        Walk();
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
        /*if (period >= walkTime / 2)
        {
            period = 0;
            walking = true;
        }
        if (walking)
        {
            RB.velocity = new Vector2(-Tile.TILE_DISTANCE.x / 2 / 0.5f, 0); // d = rt
            walkPeriod += Time.deltaTime;
        }
        else RB.velocity = Vector3.zero;
        if (walkPeriod >= 0.5f)
        {
            walking = false;
            walkPeriod = 0;
        }*/
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
            walking = false;
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

}
