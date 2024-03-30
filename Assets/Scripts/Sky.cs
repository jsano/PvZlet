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
    [HideInInspector] public bool night;

    public GameObject fog;

    private Camera cam;

    private Level l;

    // Start is called before the first frame update
    void Start()
    {
        l = FindFirstObjectByType<Level>();
        night = l.setting == Level.Setting.Night || l.setting == Level.Setting.Fog || l.setting == Level.Setting.RoofNight;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (l.fogColumn > 0)
        {
            for (int i = 1; i <= ZombieSpawner.Instance.lanes; i++)
            {
                for (int j = l.fogColumn; j <= 9; j++)
                {
                    GameObject f = Instantiate(fog, Tile.tileObjects[i, j].transform.position, Quaternion.identity);
                    f.transform.localScale = Tile.TILE_DISTANCE;
                    Tile.tileObjects[i, j].fog = f.GetComponent<Fog>();
                }
                GameObject g = Instantiate(fog, new Vector3(Tile.COL_TO_WORLD[9] + Tile.TILE_DISTANCE.x, Tile.tileObjects[i, 9].transform.position.y, 0), Quaternion.identity);
                g.transform.localScale = Tile.TILE_DISTANCE;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!night && LevelManager.status == LevelManager.Status.Start && l.conveyor.Count == 0) period += Time.deltaTime;
        if (period > interval)
        {
            period = 0;
            GameObject g = Instantiate(sun, transform.position + new Vector3(Random.Range(0, Tile.TILE_DISTANCE.x * 8f), 0, 0), Quaternion.identity);
            g.GetComponent<Sun>().ground = cam.ViewportToWorldPoint(new Vector3(0, 0.1f)).y;
        }
    }

}
