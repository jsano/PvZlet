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
    private GameObject parent;
    private int lane;
    private Vector3 dir;
    private int moveToLane;
    /// <summary> The number of tiles this projectile moves before disappearing </summary>
    private float distance;
    private int blockAtSlopeColumn;
    private Vector3 startPos;
    /// <summary> Whether this projectile has splash damage </summary>
    public Vector2 splash;
    public int targets;

    public bool pea;
    public bool sharp;

    protected Rigidbody2D RB;

    public AudioClip[] hitSFX;

    /// <summary> Required function for setting up this projectile after <c>Instantiate</c>ing </summary>
    /// <param name="parent"> The game object that fired this projectile </param>
    /// <param name="dir"> The direction this projectile travels in </param>
    /// <param name="moveToLane"> If set, this projectile moves to this lane right as it's spawned </param>
    public void Setup(GameObject parent, Vector3 dir, int moveToLane = 0, int blockAtSlopeColumn = 0)
    {
        this.parent = parent;
        this.dir = dir;
        this.moveToLane = moveToLane;
        this.blockAtSlopeColumn = blockAtSlopeColumn;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.velocity = dir.normalized * speed;
        startPos = parent.transform.position;
        if (parent.GetComponent<Plant>() != null)
        {
            lane = parent.GetComponent<Plant>().row;
            distance = parent.GetComponent<Plant>().range;
        }
        else
        {
            lane = parent.GetComponent<Zombie>().row;
            distance = 10; // Probably no puffshroom zombies
        }
        if (moveToLane != 0)
        {
            lane = moveToLane;
            startPos = new Vector3(parent.transform.position.x, Tile.tileObjects[moveToLane, Mathf.Min(9, Tile.WORLD_TO_COL(parent.transform.position.x) + 1)].transform.position.y, 0);
        }
        if (blockAtSlopeColumn == 0) blockAtSlopeColumn = Tile.WORLD_TO_COL(parent.transform.position.x) + 1;
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
        if (targets <= 0) Destroy(gameObject);
    }

    /// <summary> Called when this projectile hits an enemy. By default, it deals damage, and then disappears. Override this method if otherwise </summary>
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (targets <= 0) return;
        if (other.offset.y < 0 || other.GetComponent<BoxCollider2D>().size.y < 1 && !sharp) return; // NOTE: Maybe represent submerged with something more concrete
        if (other.gameObject.layer == LayerMask.NameToLayer("Slope"))
        {
            if ((dir.y != 0 || other.GetComponent<Tile>().row == lane) && other.GetComponent<Tile>().col == blockAtSlopeColumn) Destroy(gameObject);
            return;
        }
        if (other.GetComponent<Zombie>() != null && other.GetComponent<Zombie>().row != lane && dir.y == 0) return;
        if (other.GetComponent<Plant>() != null && other.GetComponent<Plant>().row != lane && dir.y == 0) return;

        if (hitSFX.Length > 0) SFX.Instance.Play(hitSFX[Random.Range(0, hitSFX.Length)]);
        // Prioritize shield/zomboni over zombie since they can't splash
        if (other.GetComponent<Shield>() != null)
        {
            Hit(other.GetComponent<Shield>(), dmg);                
            return;
        }
        if (other.GetComponent<Zomboni>() != null)
        {
            Hit(other.GetComponent<Zomboni>(), dmg);
            return;
        }
        // No shield, just zombie
        else
        {
            if (splash.magnitude > 0)
            {
                RaycastHit2D[] hits1 = Physics2D.BoxCastAll(transform.position, Tile.TILE_DISTANCE * splash, 0, Vector2.zero, 0, Physics2D.GetLayerCollisionMask(gameObject.layer));
                foreach (RaycastHit2D h in hits1)
                    if (h.collider.gameObject.layer != LayerMask.NameToLayer("Slope") && h.collider.GetComponent<BoxCollider2D>().size.y >= 1)
                        Hit(h.collider.GetComponent<Damagable>(), h.collider == other ? dmg : dmg / 2);
            }
            else Hit(other.GetComponent<Zombie>(), dmg);
        }
    }

    protected virtual void Hit(Damagable other, float amount)
    {
        other.ReceiveDamage(amount, gameObject, disintegrating: tag == "Fire");
        targets -= 1;
    }

    public GameObject GetParent()
    {
        return parent;
    }

    public Vector3 GetDir()
    {
        return dir;
    }

    public float GetDistance()
    {
        return distance;
    }

    public int GetMoveToLane()
    {
        return moveToLane;
    }

}
