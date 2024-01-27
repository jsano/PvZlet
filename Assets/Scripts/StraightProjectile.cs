using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightProjectile : MonoBehaviour
{

    /// <summary> The speed at which this projectile travels </summary>
    public float speed;
    //public GameObject dissolve;
    /// <summary> The amount of damage per projectile </summary>
    public float dmg;
    /// <summary> The direction this projectile travels in </summary>
    [HideInInspector] public Vector3 dir;
    /// <summary> If not 0, this projectile moves to this lane right as it's spawned </summary>
    [HideInInspector] public int moveToLane;
    /// <summary> The number of tiles this projectile moves before disappearing </summary>
    public float distance;
    private Vector3 startPos;
    /// <summary> Whether this projectile has in-lane splash damage </summary>
    public bool laneSplash;
    /// <summary> Whether this projectile has multi-lane splash damage </summary>
    public bool neighboringLaneSplash;

    public bool pea;
    public bool sharp;

    private Rigidbody2D RB;

    private bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        dir = Vector3.right;
        RB = GetComponent<Rigidbody2D>();
        RB.velocity = dir.normalized * speed;
        startPos = transform.position;
        if (moveToLane != 0) startPos = new Vector3(transform.position.x, Tile.ROW_TO_WORLD[moveToLane], 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToLane != 0 && transform.position.y != Tile.ROW_TO_WORLD[moveToLane])
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, Tile.ROW_TO_WORLD[moveToLane], 0), speed * Time.deltaTime);
        }
        if (Vector3.Distance(startPos, transform.position) / Tile.TILE_DISTANCE.x >= distance)
        {
            Destroy(gameObject);
        }
    }

    /// <summary> Called when this projectile hits an enemy. By default, it deals damage, and then disappears. Override this method if otherwise </summary>
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (hit) return;
        if (other.offset.y < 0 || other.offset.y > 0 && !sharp) return; // NOTE: Maybe represent submerged with something more concrete
        if (other.gameObject.layer == LayerMask.NameToLayer("Slope"))
        {
            Destroy(gameObject);
            return;
        }
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, transform.localScale.x, Vector2.zero, 0, Physics2D.GetLayerCollisionMask(gameObject.layer));
        // Prioritize shield over zombie
        foreach (RaycastHit2D h in hits) {
            other = h.collider;
            if (other.GetComponent<Shield>() != null)
            {
                laneSplash = false;
                Hit(other.GetComponent<Shield>());                
                return;
            }
        }
        // No shield, just zombie
        if (hits.Length > 0) Hit(hits[0].collider.GetComponent<Zombie>());
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
