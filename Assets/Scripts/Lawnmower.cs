using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lawnmower : MonoBehaviour
{

    private Rigidbody2D RB;
    private bool launched;
    public int row;

    public AudioClip mow;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > Tile.tileObjects[1, 9].transform.position.x + Tile.TILE_DISTANCE.x) Destroy(gameObject);
        if (launched) 
        {
            int c = Mathf.Clamp(Tile.WORLD_TO_COL(transform.position.x), 1, 8);
            RB.velocity = (Tile.tileObjects[row, c + 1].transform.position - Tile.tileObjects[row, c].transform.position).normalized * 5;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Zombie>().row == row)
        {
            if (!launched) SFX.Instance.Play(mow);
            launched = true;
            collider.GetComponent<Zombie>().ReceiveDamage(100000, gameObject, disintegrating: true);
        }
    }

}
