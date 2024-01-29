using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbedProjectile : MonoBehaviour
{

    /// <summary> The amount of damage per projectile </summary>
    public float dmg;
    /// <summary> The projectile only hits zombies in this lane </summary>
    [HideInInspector] public int lane;
    /// <summary> The number of tiles this projectile moves before disappearing </summary>
    [HideInInspector] public float distance;
    /// <summary> The fixed amount of time this projectile spends in the air regardless of distance </summary>
    private float airTime;

    /// <summary> Whether this projectile has in-lane splash damage </summary>
    public bool laneSplash;
    /// <summary> Whether this projectile has multi-lane splash damage </summary>
    public bool neighboringLaneSplash;

    private Rigidbody2D RB;

    private bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.velocity = new Vector2(distance / airTime, 0.5f * 9.8f * airTime); // 0 = 1/2(-9.8)t^2 + vt
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= Tile.tileObjects[lane, Tile.WORLD_TO_COL(transform.position.x)].transform.position.y - Tile.TILE_DISTANCE.y / 2)
            Destroy(gameObject);
    }

    /// <summary> Called when this projectile hits an enemy. By default, it deals damage, and then disappears. Override this method if otherwise </summary>
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (hit) return;
        Hit(other.GetComponent<Zombie>());
    }

    protected virtual void Hit(Damagable other)
    {
        if (laneSplash)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, transform.localScale.x * 2, Vector2.zero, 0, Physics2D.GetLayerCollisionMask(gameObject.layer));
            foreach (RaycastHit2D h in hits) h.collider.GetComponent<Damagable>().ReceiveDamage(dmg, gameObject);
        }
        else other.ReceiveDamage(dmg, gameObject);
        hit = true;
        Destroy(gameObject);
    }

}
