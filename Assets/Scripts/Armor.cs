using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{

    public float HP;
    private SpriteRenderer SR;
    private Zombie user;

    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        user = transform.parent.GetComponent<Zombie>();
    }

    // Update is called once per frame
    void Update()
    {
        if (user != null) SR.material.color = user.getSpriteRenderer().material.color;
        if (HP <= 0) Destroy(gameObject);
    }

    public float ReceiveDamage(float dmg)
    {
        float remaining = Mathf.Max(0, dmg - HP);
        HP -= dmg;
        return remaining;
    }

    public void DetachUser()
    {
        user = null;
    }

}
