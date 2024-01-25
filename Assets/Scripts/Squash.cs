using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squash : Plant
{

    private bool attacking;
    public Vector2 area;

    protected override void Attack(Zombie z)
    {
        if (!attacking)
        {
            attacking = true;
            HP = 100000;
            Tile.tileObjects[row, col].planted = null;
            StartCoroutine(SquashAfterDelay(z));
        }
    }

    private IEnumerator SquashAfterDelay(Zombie z)
    {
        yield return new WaitForSeconds(1);
        SR.sortingLayerName = "Armor";
        SR.sortingOrder = 1;
        Vector3 dest = z.transform.position + new Vector3(0, Tile.TILE_DISTANCE.y, 0);
        while (transform.position != dest)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest, 15 * Time.deltaTime);
            yield return null;
        }
        dest = transform.position - new Vector3(0, Tile.TILE_DISTANCE.y, 0);
        yield return new WaitForSeconds(0.25f);
        while (transform.position != dest)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest, 15 * Time.deltaTime);
            yield return null;
        }
        RaycastHit2D[] all = Physics2D.BoxCastAll(dest, area * Tile.TILE_DISTANCE, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie"));
        foreach (RaycastHit2D a in all)
        {
            a.collider.GetComponent<Zombie>().ReceiveDamage(damage, null);
        }
        Destroy(gameObject);
    }

}
