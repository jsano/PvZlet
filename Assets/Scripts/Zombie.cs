using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    public int spawnScore;
    public int damage;
    public float walkTime; // Seconds per tile
    private float period = 0;
    public float HP;
    [HideInInspector] public float eatTime = 0.5f;
    [HideInInspector] public int row = 1; // [1-5]

    public GameObject projectile;

    private Rigidbody2D RB;
    private SpriteRenderer SR;

    private GameObject eating;
    private Coroutine eatingCoroutine;

    [HideInInspector] public StatMod status;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, transform.localScale, 0, Vector2.zero, 0, LayerMask.GetMask("Plant"));
        bool nothing = true;
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject.GetComponent<Plant>().instant) continue;
            if (eating == null || hit[i].collider.gameObject != eating)
            {
                if (eatingCoroutine != null) StopCoroutine(eatingCoroutine);
                eatingCoroutine = StartCoroutine(Eat(hit[i].collider.GetComponent<Plant>()));
            }
            nothing = false;
            break;
        }
        if (nothing) Walk();
        if (HP <= 0)
        {
            Die();
        }
    }

    protected void Spawn()
    {
        transform.position = new Vector3(Tile.TILE_DISTANCE.x * 7.5f, Tile.TILE_DISTANCE.y * (3 - row), 0);
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

    protected IEnumerator Eat(Plant p)
    {
        eating = p.gameObject;
        while (p != null)
        {
            RB.velocity = Vector2.zero;
            period = 0;
            p.ReceiveDamage(damage);
            yield return new WaitForSeconds(eatTime);
        }
    }

    public virtual void ReceiveDamage(float dmg)
    {
        HP -= dmg;
        StartCoroutine(HitVisual());
    }

    protected IEnumerator HitVisual()
    {
        Color c = SR.material.color;
        SR.material.color += Color.red / 2;
        yield return new WaitForSeconds(0.1f);
        SR.material.color = c;
    }

    protected void Die()
    {
        GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>().currentBuild -= spawnScore;
        Destroy(gameObject);
    }

}
