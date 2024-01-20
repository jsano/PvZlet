using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{

    public GameObject sun;
    public float interval;
    private float period = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        period += Time.deltaTime;
        if (period > interval)
        {
            period = 0;
            Instantiate(sun, transform.position + new Vector3(Random.Range(0, Tile.TILE_DISTANCE.x * 9f), 0, 0), Quaternion.identity);
        }
    }
}
