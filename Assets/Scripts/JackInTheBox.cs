using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackInTheBox : Football
{

    public float maxTime;
    private bool exploded;

    // Update is called once per frame
    public override void Update()
    {
        if (!exploded)
        {
            maxTime -= Time.deltaTime * Random.Range(1, 4);
            base.Update();
            if (maxTime <= 0)
            {
                exploded = true;
                StartCoroutine(Explode());
            }
        }
    }

    private IEnumerator Explode()
    {
        RB.velocity = Vector3.zero;
        StopEating();
        BC.enabled = false;
        yield return new WaitForSeconds(1);
        Vector2 area = new Vector2(2.5f, 2.5f);
        GameObject g = Instantiate(projectile, transform.position, Quaternion.identity);
        g.transform.localScale = area * Tile.TILE_DISTANCE;
        int mask = hypnotized ? LayerMask.GetMask("Zombie") : LayerMask.GetMask("Plant");
        RaycastHit2D[] all = Physics2D.BoxCastAll(transform.position, area * Tile.TILE_DISTANCE, 0, Vector2.zero, 0, mask);
        foreach (RaycastHit2D a in all)
        {
            a.collider.GetComponent<Damagable>().ReceiveDamage(1000, gameObject);
        }
        Die();
    }

}
