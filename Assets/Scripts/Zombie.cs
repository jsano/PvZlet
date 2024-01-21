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

    public GameObject armor;
    public GameObject shield;
    public GameObject projectile;

    protected Rigidbody2D RB;
    protected SpriteRenderer SR;

    private GameObject eating;
    private Coroutine eatingCoroutine;

    [HideInInspector] public StatMod status;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        if (armor != null) armor = Instantiate(armor, transform, false);
        Spawn();
    }

    // Update is called once per frame
    public virtual void Update()
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
    }

    void LateUpdate()
    {
        if (HP <= 0) Die();
    }

    protected void Spawn()
    {
        transform.position = new Vector3(Tile.TILE_DISTANCE.x * 7.5f, ZombieSpawner.ROW_TO_WORLD[row], 0);
    }

    protected void Walk()
    {
        period += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
        if (period >= walkTime / 2)
        {
            period = 0;
            StartCoroutine(Walk_Helper());
        }
    }

    private IEnumerator Walk_Helper()
    {
        RB.velocity = new Vector2(-Tile.TILE_DISTANCE.x / 2 / 0.5f, 0) * ((status == null) ? 1 : status.walkMod); // d = rt
        yield return new WaitForSeconds(0.5f * ((status == null) ? 1 : 1 / status.walkMod));
        RB.velocity = Vector3.zero;
    }

    protected void WalkConstant()
    {
        RB.velocity = new Vector3(-Tile.TILE_DISTANCE.x / walkTime, 0, 0) * ((status == null) ? 1 : status.walkMod); // d = rt
    }

    protected IEnumerator Eat(Plant p)
    {
        eating = p.gameObject;
        while (p != null)
        {
            RB.velocity = Vector2.zero;
            period = 0;
            p.ReceiveDamage(damage);
            yield return new WaitForSeconds(eatTime * ((status == null) ? 1 : 1 / status.eatMod));
        }
    }

    public void ReceiveDamage(float dmg)
    {
        if (armor != null) dmg = armor.GetComponent<Armor>().ReceiveDamage(dmg);
        HP -= dmg;
        StartCoroutine(HitVisual());
    }

    protected IEnumerator HitVisual()
    {
        SR.material.color = new Color(1, 0.8f, 0.8f, 0.8f);
        yield return new WaitForSeconds(0.1f);
        SR.material.color = (status == null) ? Color.white : status.c;
    }

    protected void Die()
    {
        GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>().currentBuild -= spawnScore;
        Destroy(gameObject);
    }

    public SpriteRenderer getSpriteRenderer()
    {
        return SR;
    }

}
