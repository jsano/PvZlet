using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Damagable
{

    public float HP;
    private SpriteRenderer SR;

    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) Destroy(gameObject);
    }

    public override void ReceiveDamage(float dmg) // I really hate reusing code from Zombie.cs and I wish I could borrow it somehow. Maybe "Damageable" interface?
    {
        HP -= dmg;
        StartCoroutine(HitVisual());
    }

    protected IEnumerator HitVisual()
    {
        SR.material.color = new Color(1, 0.8f, 0.8f, 0.8f);
        yield return new WaitForSeconds(0.1f);
        SR.material.color = Color.white;
    }

}
