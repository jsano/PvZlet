using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Damagable
{

    /// <summary> The plant's sun cost </summary>
    public int cost;
    /// <summary> The plant's recharge in seconds </summary>
    public float recharge;
    /// <summary> How much damage the plant deals per attack. Irrelevant if the plant fires projectiles </summary>
    public float damage;
    /// <summary> The interval in seconds between each attack. For some, this can be the "startup" time for a singular attack </summary>
    public float atkspd;
    /// <summary> Whether this plant should have a random initial startup time </summary>
    public bool variableStartPeriod;
    private float period;
    /// <summary> How much HP the plant has. Most non-wall plants should have the same value </summary>
    public float HP;
    /// <summary> How many tiles the plant can see ahead to start attacking. Irrelevant if <c>alwaysAttack</c> is true </summary>
    public float range;
    /// <summary> Whether the plant should be allowed to attack regardless of range </summary>
    public bool alwaysAttack = false;
    /// <summary> Whether the plant is a mushroom thus nocturnal </summary>
    public bool mushroom = false;
    protected Sky sky;
    /// <summary> Whether this is an instant plant so zombies can ignore it </summary>
    public bool instant;
    /// <summary> Whether this is a grounded plant so zombies can only interact with it if their <c>hitsGround</c> is true </summary>
    public bool grounded;

    /// <summary> Which row the plant is in. Takes values between [1 - <c>ZombieSpawner.lanes</c>]. Useful for lobbing plants </summary>
    [HideInInspector] public int row;
    /// <summary> Which column the plant is in. Takes values between [1 - 9] </summary>
    [HideInInspector] public int col;

    public GameObject projectile;
    /// <summary> The right offset from the main plant where straight projectiles should naturally spawn </summary>
    protected Vector3 rightOffset;
    /// <summary> The top offset from the main plant where lobbed projectiles should naturally spawn </summary>
    protected Vector3 topOffset;

    protected SpriteRenderer SR;

    /// <summary> Any active status effect. Will be null if there's no status </summary>
    [HideInInspector] public StatMod status;

    void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        if (variableStartPeriod) period = Random.Range(0, atkspd / 2);
        rightOffset = new Vector3(Tile.TILE_DISTANCE.x / 3, 0);
        topOffset = new Vector3(0, Tile.TILE_DISTANCE.y / 2);
        sky = GameObject.Find("Sky").GetComponent<Sky>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (mushroom && !sky.night)
        {
            return;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, (range + 0.5f) * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
        if (hit || instant || alwaysAttack) {
            period += Time.deltaTime;
            if (period >= atkspd)
            {
                if (hit) Attack(hit.collider.gameObject.GetComponent<Zombie>());
                else Attack(null);
                period = 0;
            }
        }
        else
        {
            if (variableStartPeriod) period = Random.Range(0, atkspd / 2);
            else period = 0;
        }
    }

    void LateUpdate()
    {
        if (HP <= 0) Die();
    }

    /// <summary> The plant's main attack, called at most every <c>atkspd</c> seconds. Does nothing by default, and most non-walls should override this method </summary>
    /// <param name="z"> The zombie to attack at. Can be null if it's not necessary </param>
    protected virtual void Attack(Zombie z)
    {

    }

    public override void ReceiveDamage(float dmg, GameObject source, bool eat=false)
    {
        HP -= dmg;
        StartCoroutine(EatenVisual());
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected IEnumerator EatenVisual()
    {
        SR.material.color = new Color(1, 0.8f, 0.8f, 0.8f);
        yield return new WaitForSeconds(0.1f);
        SR.material.color = (status == null) ? Color.white : status.colorTint;
    }

}
