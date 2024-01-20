using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{

    public int cost;
    public float recharge;
    public float damage;
    public float atkspd;
    private float period = 0;
    public int HP;
    public float range;
    public bool alwaysAttack = false;
    public bool instant;

    [HideInInspector] public int row;
    [HideInInspector] public int col;

    public GameObject projectile;
    protected Vector3 rightOffset;
    protected Vector3 topOffset;

    protected SpriteRenderer SR;

    // Start is called before the first frame update
    public virtual void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        rightOffset = new Vector3(Tile.TILE_DISTANCE.x / 3, 0);
        topOffset = new Vector3(0, Tile.TILE_DISTANCE.y / 2);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        period += Time.deltaTime;
        if (period >= atkspd)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, (range + 0.5f) * Tile.TILE_DISTANCE.x, LayerMask.GetMask("Zombie"));
            if (hit || alwaysAttack || instant)
            { 
                Attack(hit.collider.gameObject.GetComponent<Zombie>());
                period = 0;
            }
        }
    }

    void LateUpdate()
    {
        if (HP <= 0) Die();
    }

    protected virtual void Attack(Zombie z)
    {

    }

    public void ReceiveDamage(int dmg)
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
        Color c = SR.material.color;
        SR.material.color += Color.red / 2;
        yield return new WaitForSeconds(0.1f);
        SR.material.color = c;
    }

}
