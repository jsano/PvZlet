using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Damagable
{

    public float HP;
    private float baseHP;
    private SpriteRenderer SR;
    private Color baseMaterialColor = Color.white;

    private Sprite normalSprite;
    public Sprite damagedSprite1;
    public Sprite damagedSprite2;

    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        normalSprite = SR.sprite;
        baseHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP / baseHP > 2f / 3) SR.sprite = normalSprite;
        else if (HP / baseHP > 1f / 3) SR.sprite = damagedSprite1;
        else SR.sprite = damagedSprite2;
        if (HP <= 0) Destroy(gameObject);
    }

    public override void ReceiveDamage(float dmg, GameObject source, bool eat=false) // I really hate reusing code from Zombie.cs and I wish I could borrow it somehow. Maybe "Damageable" interface?
    {
        HP -= dmg;
        StartCoroutine(HitVisual());
    }

    protected IEnumerator HitVisual()
    {
        SR.material.color = new Color(1, 0.8f, 0.8f, 0.8f);
        yield return new WaitForSeconds(0.1f);
        SR.material.color = baseMaterialColor;
    }
    
    public void Hypnotize()
    {
        baseMaterialColor = Color.magenta;
        SR.material.color = baseMaterialColor;
    }

}
