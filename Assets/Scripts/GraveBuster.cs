using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveBuster : Plant
{

    public float destroyTime;
    private GameObject grave;

    public override void Start()
    {
        base.Start();
        grave = Tile.tileObjects[row, col].ContainsGridItem("Grave");
        SFX.Instance.Play(attackSFX[Random.Range(0, attackSFX.Length)]);
    }

    // Update is called once per frame
    public override void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime < 0)
        {
            Destroy(grave);
            Destroy(gameObject);
        }
    }

}
