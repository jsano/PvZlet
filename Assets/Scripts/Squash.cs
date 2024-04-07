using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squash : Plant
{

    private bool attacked;
    public Vector2 area;

    public AudioClip[] see;
    public AudioClip squashFX;

    protected override void Attack(Zombie z)
    {
        if (!attacked)
        {
            SFX.Instance.Play(see[Random.Range(0, see.Length)]);
            attacked = true;
            HP = 100000;
            Tile.tileObjects[row, col].planted = null;
            StartCoroutine(SquashAfterDelay(z));
        }
    }

    private IEnumerator SquashAfterDelay(Zombie z)
    {
        Vector3 dest = z.transform.position;
        yield return new WaitForSeconds(1);
        SR.sortingLayerName = "Projectile";
        GetComponent<BoxCollider2D>().enabled = false;
        dest += new Vector3(0, Tile.TILE_DISTANCE.y, 0);
        if (z != null) dest.x = z.transform.position.x;
        while (transform.position != dest)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest, 25 * Time.deltaTime);
            yield return null;
        }
        dest = transform.position - new Vector3(0, Tile.TILE_DISTANCE.y, 0);
        yield return new WaitForSeconds(0.25f);
        while (transform.position != dest)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest, 25 * Time.deltaTime);
            yield return null;
        }
        RaycastHit2D[] all = Physics2D.BoxCastAll(dest, area * Tile.TILE_DISTANCE, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie"));
        foreach (RaycastHit2D a in all)
        {
            a.collider.GetComponent<Zombie>().ReceiveDamage(damage, null, disintegrating: true);
        }
        SFX.Instance.Play(squashFX);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}
