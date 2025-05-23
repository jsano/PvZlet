using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbedProjectile : MonoBehaviour
{

    /// <summary> The amount of damage per projectile </summary>
    public float dmg;
    /// <summary> The projectile only hits zombies in this lane </summary>
    [HideInInspector] public int lane;
    /// <summary> The distance this projectile moves in the air </summary>
    [HideInInspector] public Vector2 distance;
    /// <summary> The fixed amount of time this projectile spends in the air regardless of distance </summary>
    private float airTime = 1f;

    /// <summary> Whether this projectile has splash damage </summary>
    public Vector2 splash;

    private Rigidbody2D RB;

    private bool hit = false;

    public AudioClip[] hitSFX;

    // Start is called before the first frame update
    public virtual void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.velocity = new Vector2(distance.x / airTime, (distance.y - 0.5f * Physics2D.gravity.y * RB.gravityScale * Mathf.Pow(airTime, 2)) / airTime); // y = 1/2(-9.8)t^2 + vt
    }

    // Update is called once per frame
    void Update()
    {
        int c = Mathf.Min(9, Mathf.Max(1, Tile.WORLD_TO_COL(transform.position.x)));
        if (transform.position.y <= Tile.tileObjects[lane, c].transform.position.y - Tile.TILE_DISTANCE.y / 2)
            Destroy(gameObject);
    }

    void LateUpdate()
    {
        if (hit) Destroy(gameObject);
    }

    /// <summary> Called when this projectile hits an enemy. By default, it deals damage, and then disappears. Override this method if otherwise </summary>
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (hit) return;
        if (other.GetComponent<Shield>() != null) return;
        if (other.GetComponent<Zombie>() != null && other.GetComponent<Zombie>().row != lane && other.GetComponent<Zomboss>() == null) return;
        if (other.GetComponent<Plant>() != null && other.GetComponent<Plant>().row != lane) return;
        if (hitSFX.Length > 0) SFX.Instance.Play(hitSFX[Random.Range(0, hitSFX.Length)]);
        if (other.GetComponent<Damagable>() != null)
        {
            if (splash.magnitude > 0)
            {
                RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, Tile.TILE_DISTANCE * splash, 0, Vector2.zero, 0, Physics2D.GetLayerCollisionMask(gameObject.layer));
                foreach (RaycastHit2D h in hits) Hit(h.collider.GetComponent<Damagable>(), h.collider == other ? dmg : dmg / 2);
            }
            else Hit(other.GetComponent<Damagable>(), dmg);
        }
    }

    protected virtual void Hit(Damagable other, float amount)
    {
        other.ReceiveDamage(amount, gameObject);
        hit = true;
    }

}
