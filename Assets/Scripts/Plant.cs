using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plant : MonoBehaviour
{

    public int cost;
    public float recharge;
    public float damage;
    public float atkspd;
    private float period = 0;
    public int HP;
    public float range;

    public GameObject projectile;
    protected Vector3 rightOffset;
    protected Vector3 topOffset;

    // Start is called before the first frame update
    void Start()
    {
        rightOffset = new Vector3(1, 0);
        topOffset = new Vector3(1.15f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        period += Time.deltaTime;
        if (period >= atkspd)
        {
            //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range);
            //if (hit)
            //{ 
                Attack();
                period = 0;
            //}
        }
        if (HP <= 0)
        {
            Die();
        }
    }

    protected abstract void Attack();

    protected virtual void ReceiveDamage(int dmg)
    {
        HP -= dmg;
        //StartCoroutine(IFrames());
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

}
