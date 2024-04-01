using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Zombie : Damagable
{
    public string zombieName;

    [HideInInspector] public int waveNumber;
    /// <summary> How much this zombie's worth. Used for progressing waves when killed </summary>
    public int spawnScore;
    /// <summary> How much damage to deal to plants per bite </summary>
    public int damage;
    /// <summary> Amount of time in seconds it takes to fully cross 1 tile </summary>
    public float walkTime;
    protected float walkPeriod;
    protected float stepPeriod;
    private bool takingStep;
    /// <summary> How much HP the zombie has. Doesn't include armor or shields </summary>
    public float HP;
    protected float baseHP;
    protected float eatTime = 0.5f;
    private float eatPeriod;
    /// <summary> Which row the zombie is in. Takes values between [1 - <c>ZombieSpawner.lanes</c>] </summary>
    [HideInInspector] public int row = 1;
    /// <summary> Whether the zombie is able to attack grounded plants like Spikeweed </summary>
    public bool hitsGround;
    public bool aquatic;

    public GameObject armor;
    public GameObject shield;
    public GameObject projectile;
    private SpriteRenderer armorSR;
    private SpriteRenderer shieldSR;

    protected Rigidbody2D RB;
    protected SpriteRenderer SR;
    private Color baseMaterialColor = Color.white;
    protected BoxCollider2D BC;

    public bool wheels;
    public bool eatsPlants;
    /// <summary> The currently eating plant. When the plant is dead, this would likely become null </summary>
    protected GameObject eating;
    protected bool changingLanes;

    /// <summary> Any active status effect. Will be null if there's no status </summary>
    [HideInInspector] public StatMod status;
    protected bool hypnotized;
    protected bool backwards;
    public bool unchompable;

    private GameObject _text;
    [HideInInspector] public bool displayOnly;

    public AudioClip enterSFX;

    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        BC = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        if (armor != null)
        {
            armor = Instantiate(armor, transform, false);
            armor.transform.localPosition = new Vector3(0, transform.localScale.y / 2, 0);
            armorSR = armor.GetComponent<SpriteRenderer>();
            armorSR.sortingOrder = SR.sortingOrder + 1;
        }
        if (shield != null)
        {
            shield = Instantiate(shield, transform, false);
            shield.transform.localPosition = new Vector3(-transform.localScale.x / 2, 0, 0);
            shieldSR = shield.GetComponent<SpriteRenderer>();
            shieldSR.sortingOrder = SR.sortingOrder + 2;
        }
        baseHP = HP;
        if (displayOnly)
        {
            gameObject.layer = LayerMask.NameToLayer("UI");
            enabled = false;
            RB.isKinematic = true;
            RB.velocity = Vector2.zero;
            _text = Instantiate(GameObject.Find("LevelManager").transform.Find("UI").GetComponent<UI>().textBox, transform);
            _text.transform.localPosition = new Vector3(0, -1, 0);
            _text.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = zombieName;
        }
        else Spawn();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        int mask = hypnotized ? LayerMask.GetMask("Zombie") : LayerMask.GetMask("Plant");
        GameObject toEat = ClosestEatablePlant(Physics2D.BoxCastAll(transform.position, new Vector2(Mathf.Abs(transform.localScale.x), 1), 0, Vector2.zero, 0, mask));
        if (changingLanes)
        {
            eating = null;
            eatPeriod = eatTime / 2;
        }
        else if (toEat == null)
        {
            eating = null;
            eatPeriod = eatTime / 2;
            Walk();
        }
        else
        {
            eating = toEat;
            Eat(toEat);
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
        if (enterSFX != null) SFX.Instance.Play(enterSFX, true);
        if (transform.position == Vector3.zero) transform.position = new Vector3(Tile.COL_TO_WORLD[9] + Tile.TILE_DISTANCE.x, Tile.tileObjects[row, 9].transform.position.y, 0);
    }

    /// <summary> The zombie's staggered walking behavior. Every <c>walkTime/3</c> seconds, it moves 1/3 of a tile. Factors in movement stat effects </summary>
    protected virtual void Walk()
    {
        walkPeriod += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
        if (walkPeriod >= walkTime / 3)
        {
            walkPeriod = 0;
            takingStep = true;
        }
        if (takingStep)
        {
            stepPeriod += Time.deltaTime * ((status == null) ? 1 : status.walkMod);
            int c = Mathf.Clamp(Tile.WORLD_TO_COL(transform.position.x), 2, 9);
            RB.velocity = (Tile.tileObjects[row, c - 1].transform.position - Tile.tileObjects[row, c].transform.position) / 3;
            RB.velocity /= walkTime / 6; // d = rt
            RB.velocity *= transform.localScale.x * ((status == null) ? 1 : status.walkMod); 
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
        int c = Mathf.Clamp(Tile.WORLD_TO_COL(transform.position.x), 2, 9);
        RB.velocity = Tile.tileObjects[row, c - 1].transform.position - Tile.tileObjects[row, c].transform.position;
        RB.velocity /= walkTime; // d = rt
        RB.velocity *= transform.localScale.x * ((status == null) ? 1 : status.walkMod);
    }

    /// <summary> Given a list of plants in range, gets the first interactable one </summary>
    /// <returns> The first interactable plant gameobject. null if there's no eligible plant </returns>
    /// <param name="hit"> The ist of plants in range given by some ray/boxcast </param>
    protected GameObject ClosestEatablePlant(RaycastHit2D[] hit)
    {
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.GetComponent<Player>() != null) return hit[i].collider.gameObject;
            if (hit[i].collider.GetComponent<Zombie>() != null) return hit[i].collider.gameObject;
            Plant p = hit[i].collider.GetComponent<Plant>();
            if (p.isActiveInstant() || p.row != row || p.grounded && !hitsGround) continue;
            return Tile.tileObjects[p.row, p.col].GetEatablePlant(wheels || hitsGround);
        }
        return null;
    }

    /// <summary> The zombie's eating behavior per frame. Every <c>eatTime</c> seconds, it deals <c>damage</c> to the plant. Factors in eating stat effects </summary>
    /// <param name="p"> The plant being eaten </param>
    protected virtual void Eat(GameObject p)
    {
        if (!wheels) ResetWalk();
        eatPeriod += Time.deltaTime * ((status == null) ? 1 : status.eatMod);
        if (eatPeriod >= eatTime)
        {
            eatPeriod = 0;
            if (LevelManager.status != LevelManager.Status.Lost && SFX.Instance.zombieEat.Length > 0)
            {
                if (p.GetComponent<Nut>() != null) SFX.Instance.Play(SFX.Instance.zombieEatNut);
                else if (!wheels) SFX.Instance.Play(SFX.Instance.zombieEat[UnityEngine.Random.Range(0, SFX.Instance.zombieEat.Length)]);
            }
            float rem = p.GetComponent<Damagable>().ReceiveDamage(damage, gameObject, eatsPlants);
            if (rem <= 0 && !wheels) SFX.Instance.Play(SFX.Instance.gulp);
        }
    }

    public override float ReceiveDamage(float dmg, GameObject source, bool eat = false, bool disintegrating = false)
    {
        // Adjust for zombies
        // NOTE: adjust plant HP and remove this in the future
        if (source != null && source.GetComponent<Zombie>() != null) dmg *= 4;
        if (status != null && source != null && status.removedBy == source.tag) status.Remove();
        // Any armor will take priority over the main zombie
        if (armor != null) dmg = armor.GetComponent<Armor>().ReceiveDamage(dmg, disintegrating);
        HP -= dmg;
        StartCoroutine(HitVisual());
        return HP;
    }

    protected IEnumerator HitVisual()
    {
        SR.material.color = new Color(1, 0.8f, 0.8f, 0.8f);
        yield return new WaitForSeconds(0.1f);
        SR.material.color = (status == null) ? baseMaterialColor : status.colorTint;
    }

    /// <summary> Updates the spawner's progression score, and disappears </summary>
    public virtual void Die()
    {
        ZombieSpawner.Instance.SubtractBuild(spawnScore, waveNumber);
        Destroy(shield);
        Destroy(gameObject);
    }

    public virtual void Hypnotize()
    {
        ResetWalk();
        hypnotized = true;
        SFX.Instance.Play(SFX.Instance.hypnotize);
        gameObject.layer = LayerMask.NameToLayer("Plant");
        baseMaterialColor = Color.magenta;
        SR.material.color = baseMaterialColor;
        if (shield != null) shield.GetComponent<Shield>().Hypnotize();
        ZombieSpawner.Instance.SubtractBuild(spawnScore, waveNumber);
        spawnScore = 0;
    }

    public void ResetWalk()
    {
        walkPeriod = walkTime / 6;
        stepPeriod = 0;
        RB.velocity = Vector2.zero;
        takingStep = false;
    }

    public void MoveToLane(int lane, float delay)
    {
        changingLanes = true;
        StartCoroutine(MoveLaneHelper(lane, delay));
    }

    private IEnumerator MoveLaneHelper(int lane, float delay)
    {
        yield return new WaitForSeconds(delay);
        float targetY = Tile.tileObjects[lane, Tile.WORLD_TO_COL(transform.position.x)].transform.position.y;
        while (Mathf.Abs(transform.position.y - targetY) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetY, 0), 2f * Time.deltaTime * ((status == null) ? 1 : status.walkMod));
            yield return null;
        }
        row = lane;
        changingLanes = false;
    }

    void OnMouseEnter()
    {
        _text.SetActive(true);
    }

    void OnMouseExit()
    {
        _text.SetActive(false);
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
