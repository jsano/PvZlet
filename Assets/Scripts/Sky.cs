using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{

    public GameObject sun;
    /// <summary> The amount of time in seconds between falling suns </summary>
    public float interval;
    private float period = 0;

    /// <summary> If nighttime, no sun falls and mushrooms sleep </summary>
    public bool night;

    /// <summary> If >0, fog will cover this column and beyond </summary>
    public int fogColumn;
    public GameObject fog;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (fogColumn > 0)
        {
            for (int i = 1; i <= GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>().lanes; i++)
            {
                for (int j = fogColumn; j <= 9; j++)
                {
                    GameObject f = Instantiate(fog, new Vector3(Tile.COL_TO_WORLD[j], Tile.ROW_TO_WORLD[i], 0), Quaternion.identity);
                    f.transform.localScale = Tile.TILE_DISTANCE;
                }
                GameObject g = Instantiate(fog, new Vector3(Tile.COL_TO_WORLD[9] + Tile.TILE_DISTANCE.x, Tile.ROW_TO_WORLD[i], 0), Quaternion.identity);
                g.transform.localScale = Tile.TILE_DISTANCE;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!night) period += Time.deltaTime;
        if (period > interval)
        {
            period = 0;
            GameObject g = Instantiate(sun, transform.position + new Vector3(Random.Range(0, Tile.TILE_DISTANCE.x * 8f), 0, 0), Quaternion.identity);
            g.GetComponent<Sun>().ground = cam.ViewportToWorldPoint(new Vector3(0, 0.1f)).y;
        }
    }
}
