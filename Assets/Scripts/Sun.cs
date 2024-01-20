using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{

    public int sunValue;
    public float lifetime;
    [HideInInspector] public float ground = -999;
    public float speed;

    private Camera cam;

    private SpriteRenderer SR;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (ground == -999) ground = cam.ViewportToWorldPoint(new Vector3(0, 0.1f)).y;
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > ground) transform.Translate(Vector3.down * speed * Time.deltaTime);
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            SR.color -= new Color(0, 0, 0, 2 * Time.deltaTime);
            if (SR.color.a <= 0) Destroy(gameObject);
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButton(0)) {
            PlantBuilder.sun += sunValue;
            Destroy(gameObject);
        }
    }

}
