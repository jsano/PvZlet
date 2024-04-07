using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marigold : CherryBomb
{

    /// <summary> Reduces all zombies' HP to a third in a 3x3 area, and then disappears </summary>
    protected override void Attack(Zombie z)
    {
        GameObject g = Instantiate(projectile, transform.position, Quaternion.identity);
        g.transform.localScale = area * Tile.TILE_DISTANCE;
        RaycastHit2D[] all = Physics2D.BoxCastAll(transform.position, area * Tile.TILE_DISTANCE, 0, Vector2.zero, 0, LayerMask.GetMask("Zombie", "ExplosivesOnly"));
        foreach (RaycastHit2D a in all)
        {
            // Can't remove active ladders
            if (a.collider.GetComponent<Shield>() != null) continue;
            z = a.collider.GetComponent<Zombie>();
            if (a.transform.localScale.x < 1) z.Die();
            z.ReceiveDamage(z.HP / 3 * 2, gameObject, disintegrating: true);
            if (z.armor != null) z.armor.GetComponent<Armor>().ReceiveDamage(z.armor.GetComponent<Armor>().HP / 3 * 2, gameObject);
            if (z.shield != null) z.shield.GetComponent<Shield>().HP /= 3;
            z.transform.localScale /= 2;
        }
        Destroy(gameObject);
    }

}
