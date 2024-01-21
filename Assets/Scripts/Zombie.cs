using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    /// <summary> How much this zombie's worth. Used for progressing waves when killed </summary>
    public int spawnScore;
    /// <summary> How much damage to deal to plants per bite </summary>
    public int damage;
    /// <summary> Amount of time in seconds it takes to fully cross 1 tile </summary>
    public float walkTime;
    private float period = 0;
    /// <summary> How much HP the zombie has. Doesn't include armor or shields </summary>
    public float HP;
    private float eatTime = 0.5f;
    /// <summary> Which row the zombie is in. Takes values between [1 - <c>ZombieSpawner.lanes</c>] </summary>
    [HideInInspector] public int row = 1;
    /// <summary> Whether the zombie is able to attack grounded plants like Spikeweed </summary>
    public bool hitsGround;

    public GameObject armor;
    public GameObject shield;
    public GameObject projectile;

    protected Rigidbody2D RB;
    protected SpriteRenderer SR;

    /// <summary> The currently eating plant. When the plant is dead, this would likely become null </summary>
    private GameObject eating;
    private Coroutine eatingCoroutine;

    /// <summary> Any active status effect. Will be null if there's no status </summary>
    [HideInInspector] public StatMod status;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        if (armor != null)
        {
            armor = Instantiate(armor, transform, false);
            armor.transform.localPosition = new Vector3(0, transform.localScale.y / 2, 0);
        }
        if (shield != null)
        {
            shield = Instantiate(shield, transform, false);
            shield.transform.localPosition = new Vector3(-transform.localScale.x / 2, 0, 0);
        }
        Spawn();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        GameObject toEat = ClosestEatablePlant(Physics2D.BoxCastAll(transform.position, transform.localScale, 0, Vector2.zero, 0, LayerMask.GetMask("Plant")));
        if (toEat == null) Walk();
        else
        {
            if (eating == null || toEat != eating)
            {
                if (eatingCoroutine != null) StopCoroutine(eatingCoroutine);
                eatingCoroutine = StartCoroutine(Eat(toEat.GetComponent<Plant>()));
            }
        }
    }

    void LateUpdate()
    {
        if (HP <= 0) Die();
    }

    /// <summary> How the zombie should enter the lawn. Appears at the rightmost lane by default. Override this method if otherwise </summary>
    protected void Spawn()
    {
        transform.position = new Vector3(Tile.TILE_DISTANCE.x * 7.5f, ZombieSpawner.ROW_TO_WORLD[row], 0);
    }

    /// <summary> The zombie's staggered walking behavior. Every <c>walkTime/2</c> seconds, it moves half of a tile. Factors in movement stat effects </summary>
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

    /// <summary> The zombie's constant-speed walking behavior. Factors in movement stat effects </summary>
    protected void WalkConstant()
    {
        RB.velocity = new Vector3(-Tile.TILE_DISTANCE.x / walkTime, 0, 0) * ((status == null) ? 1 : status.walkMod); // d = rt
    }

    /// <summary> Given a list of plants in range, gets the first interactable one </summary>
    /// <returns> The first interactable plant gameobject. null if there's no eligible plant </returns>
    /// <param name="hit"> The ist of plants in range given by some ray/boxcast </param>
    protected GameObject ClosestEatablePlant(RaycastHit2D[] hit)
    {
        for (int i = 0; i < hit.Length; i++)
        {
            Plant p = hit[i].collider.gameObject.GetComponent<Plant>();
            if (p.instant || p.grounded && !hitsGround) continue;
            return hit[i].collider.gameObject;
        }
        return null;
    }

    /// <summary> The zombie's eating behavior. Every <c>eatTime</c> seconds, it deals <c>damage</c> to the plant. Factors in eating stat effects </summary>
    /// <param name="p"> The plant being eaten </param>
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

    /// <summary> Called when something deals damage to this zombie. Any armor will take priority over the main zombie </summary>
    /// <param name="dmg"> How much damage to deal </param>
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
        SR.material.color = (status == null) ? Color.white : status.colorTint;
    }

    /// <summary> Updates the spawner's progression score, and disappears </summary>
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
