using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{

    private Rigidbody2D RB;
    private float startY;
    private Camera cam;

    public Image whiteScreen;

    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
        RB = GetComponent<Rigidbody2D>();
        RB.velocity = new Vector3(Random.Range(-3f, 3f), 5);
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < startY)
        {
            RB.gravityScale = 0;
            RB.velocity = Vector3.zero;
        }
    }

    void OnMouseDown()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(Center());
    }

    private IEnumerator Center()
    {
        float period = 0;
        while (period < 4)
        {
            transform.localScale += Vector3.one * Time.deltaTime / 2;
            RB.velocity = (cam.ViewportToWorldPoint(new Vector2(0.5f, 0.5f)) - transform.position) * 0.75f;
            period += Time.deltaTime;
            yield return null;
        }
        while (whiteScreen.color.a < 1)
        {
            whiteScreen.color += new Color(0, 0, 0, Time.deltaTime / 3);
            yield return null;
        }
        GameObject.Find("UI").GetComponent<UI>().MainMenu();
    }

}
