using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{

    public float HP;
    private float baseHP;
    private SpriteRenderer SR;
    private Zombie user;

    private Sprite normalSprite;
    public Sprite damagedSprite1;
    public Sprite damagedSprite2;

    public AudioClip[] hitSFX;

    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        user = transform.parent.GetComponent<Zombie>();
        normalSprite = SR.sprite;
        baseHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        if (user != null) SR.material.color = user.getSpriteRenderer().material.color;
        if (HP / baseHP > 2f / 3) SR.sprite = normalSprite;
        else if (HP / baseHP > 1f / 3) SR.sprite = damagedSprite1;
        else SR.sprite = damagedSprite2;
        if (HP <= 0) Destroy(gameObject);
    }

    public float ReceiveDamage(float dmg, bool disintegrating)
    {
        if (!disintegrating) if (hitSFX.Length > 0) SFX.Instance.Play(hitSFX[Random.Range(0, hitSFX.Length)]);
        float remaining = Mathf.Max(0, dmg - HP);
        HP -= dmg;
        return remaining;
    }

    public void DetachUser()
    {
        user = null;
    }

}
