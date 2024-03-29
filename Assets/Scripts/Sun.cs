using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{

    /// <summary> How much sun to give when collected </summary>
    public int sunValue;
    /// <summary> The amount of time in seconds to last before disappearing </summary>
    public float lifetime;
    /// <summary> The y-value in world units of its perceived ground level to stop falling </summary>
    [HideInInspector] public float ground;
    /// <summary> How fast to fall </summary>
    public float speed;
    public AudioClip sound;

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
        if (Input.GetMouseButton(0)) Collect();
    }

    public void Collect()
    {
        SFX.Instance.Play(sound);
        PlantBuilder.sun += sunValue;
        Destroy(gameObject);
    }

}
