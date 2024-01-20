using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{

    public GameObject sun;
    public float interval;
    private float period = 0;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        period += Time.deltaTime;
        if (period > interval)
        {
            period = 0;
            GameObject g = Instantiate(sun, transform.position + new Vector3(Random.Range(0, Tile.TILE_DISTANCE.x * 8f), 0, 0), Quaternion.identity);
            g.GetComponent<Sun>().ground = cam.ViewportToWorldPoint(new Vector3(0, 0.1f)).y;
        }
    }
}
