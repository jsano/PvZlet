using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{

    public int sunValue;
    public float lifetime;
    [HideInInspector] public float ground;
    public float speed;

    private SpriteRenderer SR;

    // Start is called before the first frame update
    void Start()
    {
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
