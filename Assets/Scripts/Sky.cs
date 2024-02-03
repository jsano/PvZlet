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

    /// <summary> If >0, fog will cover this column and beyond </summary>
    public int fogColumn;
    public GameObject fog;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        Level.Setting setting = FindFirstObjectByType<Level>().setting;
        night = setting == Level.Setting.Night || setting == Level.Setting.Fog || setting == Level.Setting.RoofNight;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (fogColumn > 0)
        {
            for (int i = 1; i <= GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>().lanes; i++)
            {
                for (int j = fogColumn; j <= 9; j++)
                {
                    GameObject f = Instantiate(fog, Tile.tileObjects[i, j].transform.position, Quaternion.identity);
                    f.transform.localScale = Tile.TILE_DISTANCE;
                    Tile.tileObjects[i, j].fog = f;
                }
                GameObject g = Instantiate(fog, new Vector3(Tile.COL_TO_WORLD[9] + Tile.TILE_DISTANCE.x, Tile.tileObjects[i, 9].transform.position.y, 0), Quaternion.identity);
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

    public void ClearFog()
    {
        for (int i = 1; i <= GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>().lanes; i++)
        {
            for (int j = 1; j <= 9; j++)
            {
                if (Tile.tileObjects[i, j].fog != null)
                {
                    SpriteRenderer sr = Tile.tileObjects[i, j].fog.GetComponent<SpriteRenderer>();
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
                }
            }
        }
        StopAllCoroutines();
        StartCoroutine(BringFogBack());
    }

    private IEnumerator BringFogBack()
    {
        yield return new WaitForSeconds(45);
        for (int i = 1; i <= GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>().lanes; i++)
        {
            for (int j = 1; j <= 9; j++)
            {
                if (Tile.tileObjects[i, j].fog != null)
                {
                    SpriteRenderer sr = Tile.tileObjects[i, j].fog.GetComponent<SpriteRenderer>();
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
                }
            }
        }
    }

}
