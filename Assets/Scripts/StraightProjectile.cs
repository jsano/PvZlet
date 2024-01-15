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
    public float distance;
    private Vector3 startPos;
    public bool laneSplash;
    public bool neighboringLaneSplash;

    private Rigidbody2D RB;

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

    /// <summary> Called when this projectile hits either an enemy, enemy projectile, or ground, where it disappears in all cases </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Zombie>() != null)
        {
            other.GetComponent<Zombie>().ReceiveDamage(dmg);
        }
        // I was planning to have a particle system for when the projecile hits something to have a cool shatter effect
        /*GameObject p0 = Instantiate(dissolve, transform.position, transform.rotation);
        ParticleSystem.MainModule p = p0.GetComponent<ParticleSystem>().main;
        p.startColor = GetComponent<SpriteRenderer>().color;
        p0.GetComponent<ParticleSystemRenderer>().sortingOrder = layer;*/
        Destroy(gameObject);
    }

}
