using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Zombie : Damagable
{
    /// <summary> How much this zombie's worth. Used for progressing waves when killed </summary>
    public int spawnScore;
    /// <summary> How much damage to deal to plants per bite </summary>
    public int damage;
    /// <summary> Amount of time in seconds it takes to fully cross 1 tile </summary>
    public float walkTime;
    protected float period;
    protected float stepPeriod;
    private bool takingStep;
    /// <summary> How much HP the zombie has. Doesn't include armor or shields </summary>
    public float HP;
    [HideInInspector] public float eatTime = 0.5f;
    /// <summary> Which row the zombie is in. Takes values between [1 - <c>ZombieSpawner.lanes</c>] </summary>
    [HideInInspector] public int row = 1;
    /// <summary> Whether the zombie is able to attack grounded plants like Spikeweed </summary>
    public bool hitsGround;
    public bool aquatic;

    public GameObject armor;
    public GameObject shield;
    public GameObject projectile;

    protected Rigidbody2D RB;
    protected SpriteRenderer SR;
    private Color baseMaterialColor = Color.white;
    protected BoxCollider2D BC;
    protected ZombieSpawner ZS;

    public bool wheels;
    public bool eatsPlants;
    /// <summary> The currently eating plant. When the plant is dead, this would likely become null </summary>
    private GameObject eating;
    private Coroutine eatingCoroutine;

    /// <summary> Any active status effect. Will be null if there's no status </summary>
    [HideInInspector] public StatMod status;
    protected bool hypnotized;
    protected bool backwards;

    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        BC = GetComponent<BoxCollider2D>();
        ZS = GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
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
        int mask = hypnotized ? LayerMask.GetMask("Zombie") : LayerMask.GetMask("Plant");
        // NOTE: (1, 1, 1) and not localscale because of hypnotized -1 x-scale
        GameObject toEat = ClosestEatablePlant(Physics2D.BoxCastAll(transform.position, Vector3.one, 0, Vector2.zero, 0, mask));
        if (toEat == null)
        {
            StopEating();
            Walk();
        }
        else
        {
            if (status != null && status.eatMod == 0) StopEating();
            else if (eating == null || toEat != eating)
            {
                StopEating();
                eatingCoroutine = StartCoroutine(Eat(toEat.GetComponent<Damagable>()));
            }
        }   
    }

    public virtual void LateUpdate()
    {
        if (HP <= 0) Die();
        if (hypnotized || backwards)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, 1);
            if (Tile.COL_TO_WORLD[9] + Tile.TILE_DISTANCE.x <= transform.position.x) Die();
        }
    }

    /// <summary> How the zombie should enter the lawn. Appears at the rightmost lane by default. Override this method if otherwise </summary>
    protected virtual void Spawn()
    {
        if (transform.position == Vector3.zero) transform.position = new Vector3(Tile.COL_TO_WORLD[9] + Tile.TILE_DISTANCE.x, Tile.ROW_TO_WORLD[row], 0);
    }

    /// <summary> The zombie's staggered walking behavior. Every <c>walkTime/3</c> seconds, it moves 1/3 of a tile. Factors in movement stat effects </summary>
    protected virtual void Walk()
    {
        period += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
        if (period >= walkTime / 3)
        {
            period = 0;
            takingStep = true;
        }
        if (takingStep)
        {
            stepPeriod += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
            RB.velocity = new Vector2(-Tile.TILE_DISTANCE.x / 3 / (walkTime / 6), 0) * ((status == null) ? 1 : status.walkMod); // d = rt
            if (hypnotized || backwards) RB.velocity *= -1;
            if (stepPeriod >= walkTime / 6)
            {
                takingStep = false;
                RB.velocity = Vector3.zero;
                stepPeriod = 0;
            }
        }
    }

    /// <summary> The zombie's constant-speed walking behavior. Factors in movement stat effects </summary>
    protected void WalkConstant()
    {
        RB.velocity = new Vector3(-Tile.TILE_DISTANCE.x / walkTime, 0, 0) * ((status == null) ? 1 : status.walkMod); // d = rt
        if (hypnotized || backwards) RB.velocity *= -1;
    }

    /// <summary> Given a list of plants in range, gets the first interactable one </summary>
    /// <returns> The first interactable plant gameobject. null if there's no eligible plant </returns>
    /// <param name="hit"> The ist of plants in range given by some ray/boxcast </param>
    protected GameObject ClosestEatablePlant(RaycastHit2D[] hit)
    {
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.GetComponent<Zombie>() != null) return hit[i].collider.gameObject;
            Plant p = hit[i].collider.GetComponent<Plant>();
            if (p.isActiveInstant() || p.grounded && !hitsGround) continue;
            return Tile.tileObjects[row, p.col].planted;
        }
        return null;
    }

    /// <summary> The zombie's eating behavior. Every <c>eatTime</c> seconds, it deals <c>damage</c> to the plant. Factors in eating stat effects </summary>
    /// <param name="p"> The plant being eaten </param>
    protected virtual IEnumerator Eat(Damagable p)
    {
        eating = p.gameObject;
        ResetWalk();
        while (p != null)
        {
            if (status != null && status.eatMod == 0) break;
            if (!wheels) RB.velocity = Vector2.zero;
            period = 0;
            p.ReceiveDamage(damage, gameObject, eatsPlants);
            yield return new WaitForSeconds(eatTime * ((status == null) ? 1 : 1 / status.eatMod));
        }
    }

    public override void ReceiveDamage(float dmg, GameObject source, bool eat=false)
    {
        // Adjust for zombies
        // NOTE: adjust plant HP and remove this in the future
        if (source != null && source.GetComponent<Zombie>() != null) dmg *= 4;
        if (status != null && source != null && status.removedBy == source.tag) status.Remove();
        // Any armor will take priority over the main zombie
        if (armor != null) dmg = armor.GetComponent<Armor>().ReceiveDamage(dmg);
        HP -= dmg;
        StartCoroutine(HitVisual());
    }

    protected IEnumerator HitVisual()
    {
        SR.material.color = new Color(1, 0.8f, 0.8f, 0.8f);
        yield return new WaitForSeconds(0.1f);
        SR.material.color = (status == null) ? baseMaterialColor : status.colorTint;
    }

    /// <summary> Stops all eating processes and forgets what plant the zombie was currently eating </summary>
    public void StopEating()
    {
        eating = null;
        if (eatingCoroutine != null) StopCoroutine(eatingCoroutine);
    }

    /// <summary> Updates the spawner's progression score, and disappears </summary>
    protected virtual void Die()
    {
        GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>().currentBuild -= spawnScore;
        Destroy(shield);
        Destroy(gameObject);
    }

    public virtual void Hypnotize()
    {
        StopEating();
        ResetWalk();
        hypnotized = true;
        gameObject.layer = LayerMask.NameToLayer("Plant");
        baseMaterialColor = Color.magenta;
        SR.material.color = baseMaterialColor;
        if (shield != null) shield.GetComponent<Shield>().Hypnotize();
        GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>().currentBuild -= spawnScore;
        spawnScore = 0;
    }

    public void ResetWalk()
    {
        period = 0;
        stepPeriod = 0;
        RB.velocity = Vector2.zero;
        takingStep = false;
    }

    public SpriteRenderer getSpriteRenderer()
    {
        return SR;
    }

    public Color getBaseColor()
    {
        return baseMaterialColor;
    }

    public bool isEating()
    {
        return eating != null;
    }

}
