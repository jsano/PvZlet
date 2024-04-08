using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombotBall : Zombie
{

    private bool grown;
    public GameObject weaknessPlant;
    private GameObject child;

    protected override void Spawn()
    {
        child = transform.GetChild(0).gameObject;
        transform.localScale = Vector3.zero;
        StartCoroutine(Grow());
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!grown) return;
        if (status != null) status.Remove();
        child.transform.Rotate(0, 0, 30 * Time.deltaTime);
        GameObject toEat = ClosestEatablePlant(Physics2D.BoxCastAll(transform.position, Vector2.one * 2, 0, Vector2.zero, 0, LayerMask.GetMask("Plant")));
        if (toEat != null) Eat(toEat);
        WalkConstant();
        RaycastHit2D[] l = Physics2D.BoxCastAll(transform.position, Vector2.one * 2, 0, Vector2.zero, 0, LayerMask.GetMask("Lawnmower"));
        foreach (RaycastHit2D hit in l) if (hit.collider.GetComponent<Lawnmower>().row == row) Destroy(hit.collider.gameObject);
        if (transform.position.x <= -15) Destroy(gameObject);
    }

    private IEnumerator Grow()
    {
        float frame = 0;
        while (transform.localScale.x < 2.5f)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 2.5f, frame);
            frame += Time.deltaTime * 1f;
            yield return null;
        }
        grown = true;
    }

    public override float ReceiveDamage(float dmg, GameObject source, bool eat = false, bool disintegrating = false)
    {
        if (source.name.StartsWith(weaknessPlant.name)) Destroy(gameObject);
        return 0;
    }

}
