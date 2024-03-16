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
    /// <summary> Whether this projectile has splash damage </summary>
    public Vector2 splash;

    public bool pea;
    public bool sharp;

    protected Rigidbody2D RB;

    private bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        if (dir == Vector3.zero) dir = Vector3.right;
        RB = GetComponent<Rigidbody2D>();
        RB.velocity = dir.normalized * speed;
        startPos = transform.position;
        if (moveToLane != 0) startPos = new Vector3(transform.position.x, Tile.tileObjects[moveToLane, Mathf.Min(9, Tile.WORLD_TO_COL(transform.position.x) + 1)].transform.position.y, 0);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (moveToLane != 0 && transform.position.y != startPos.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, startPos.y, 0), speed * Time.deltaTime);
        }
        if (Vector3.Distance(startPos, transform.position) / Tile.TILE_DISTANCE.x >= distance)
        {
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        if (hit) Destroy(gameObject);
    }

    /// <summary> Called when this projectile hits an enemy. By default, it deals damage, and then disappears. Override this method if otherwise </summary>
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (hit) return;
        if (other.offset.y < 0 || other.offset.y > 0 && !sharp) return; // NOTE: Maybe represent submerged with something more concrete
        if (other.gameObject.layer == LayerMask.NameToLayer("Slope"))
        {
            if (transform.position.y != startPos.y) return;
            if (gameObject.tag == "Star")
            {
                if (dir == Vector3.up || dir == Vector3.down || dir == Vector3.left) return;
                if (Tile.WORLD_TO_COL(transform.position.x) - Tile.WORLD_TO_COL(startPos.x) >= 2) Destroy(gameObject);
                else return;
            }
            if (!sharp) Destroy(gameObject); // NOTE: Very situational, ideally only cactus spikes should ignore
            return;
        }
        // Prioritize shield/zomboni over zombie since they can't splash
        if (other.GetComponent<Shield>() != null)
        {
            Hit(other.GetComponent<Shield>());                
            return;
        }
        if (other.GetComponent<Zomboni>() != null)
        {
            Hit(other.GetComponent<Zomboni>());
            return;
        }
        // No shield, just zombie
        else
        {
            if (splash.magnitude > 0)
            {
                RaycastHit2D[] hits1 = Physics2D.BoxCastAll(transform.position, Tile.TILE_DISTANCE * splash, 0, Vector2.zero, 0, Physics2D.GetLayerCollisionMask(gameObject.layer));
                foreach (RaycastHit2D h in hits1) if (h.collider.gameObject.layer != LayerMask.NameToLayer("Slope") && h.collider.offset.y == 0) Hit(h.collider.GetComponent<Damagable>());
            }
            else Hit(other.GetComponent<Zombie>());
        }
    }

    protected virtual void Hit(Damagable other)
    {
        other.ReceiveDamage(dmg, gameObject);
        hit = true;
    }

}
