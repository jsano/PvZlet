using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkStorm : MonoBehaviour
{

    public float interval;
    private float period;
    private SpriteRenderer SR;

    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        period = interval - 1;
    }

    // Update is called once per frame
    void Update()
    {
        period += Time.deltaTime;
        if (period > interval)
        {
            period = 0;
            StartCoroutine(Flash());
        }
    }

    private IEnumerator Flash()
    {
        SR.color = Color.white;
        float whiteToGray = 0;
        while (SR.color.a > 0.5f)
        {
            SR.color = Color.Lerp(Color.white, new Color(0, 0, 0, 0.5f), whiteToGray * 3);
            whiteToGray += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        float grayToBlack = 0;
        while (SR.color.a < 1)
        {
            SR.color = Color.Lerp(new Color(0, 0, 0, 0.5f), Color.black, grayToBlack / 2);
            grayToBlack += Time.deltaTime;
            yield return null;
        }
    }

}
