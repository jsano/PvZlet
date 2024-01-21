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
    /// <summary> The number of tiles this projectile moves before disappearing </summary>
    public float distance;
    private Vector3 startPos;
    /// <summary> Whether this projectile has in-lane splash damage </summary>
    public bool laneSplash;
    /// <summary> Whether this projectile has multi-lane splash damage </summary>
    public bool neighboringLaneSplash;

    private Rigidbody2D RB;

    private bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        dir = Vector3.right;
        RB = GetComponent<Rigidbody2D>();
        RB.velocity = dir.normalized * speed;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(startPos, transform.position) / Tile.TILE_DISTANCE.x >= distance)
        {
            Destroy(gameObject);
        }
    }

    /// <summary> Called when this projectile hits an enemy, where it registers a hit and disappears </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Zombie>() != null && !hit)
        {
            Hit(other.GetComponent<Zombie>());
            hit = true;
        }
    }

    /// <summary> The projectile's behavior when hitting a zombie. By default, it deals damage to the zombie, and then disappears. Override this method if otherwise </summary>
    protected virtual void Hit(Zombie other)
    {
        other.ReceiveDamage(dmg);
        // I was planning to have a particle system for when the projecile hits something to have a cool shatter effect
        /*GameObject p0 = Instantiate(dissolve, transform.position, transform.rotation);
        ParticleSystem.MainModule p = p0.GetComponent<ParticleSystem>().main;
        p.startColor = GetComponent<SpriteRenderer>().color;
        p0.GetComponent<ParticleSystemRenderer>().sortingOrder = layer;*/
        Destroy(gameObject);
    }

}
