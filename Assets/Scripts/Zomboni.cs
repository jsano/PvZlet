using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zomboni : Football
{

    private int snowedCol = 10;

    public AudioClip[] hitSFX;
    public AudioClip explosion;

    // Update is called once per frame
    public override void Update()
    {
        int c = Tile.WORLD_TO_COL(transform.position.x + Tile.TILE_DISTANCE.x / 2);
        if (c <= 9 && c > 1 && c < snowedCol)
        {
            GameObject snow = Tile.tileObjects[row, c].ContainsGridItem("Snow");
            Destroy(snow);
            Tile.tileObjects[row, c].Place(projectile);
            snowedCol = c;
        }
        base.Update();
    }

    public override float ReceiveDamage(float dmg, GameObject source, bool eat = false, bool disintegrating = false)
    {
        SFX.Instance.Play(hitSFX[Random.Range(0, hitSFX.Length)]);
        return base.ReceiveDamage(dmg, source, eat, disintegrating);
    }

    public override void Die()
    {
        SFX.Instance.Play(explosion);
        base.Die();
    }

}
