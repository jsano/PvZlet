using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : UI
{

    public Image blackScreen;
    public TextMeshProUGUI text;
    public GameObject options;

    public override void Start()
    {
        GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        GetComponent<Canvas>().sortingLayerName = "Sun";
        GetComponent<Canvas>().sortingOrder = 1;
    }

    public override void Update()
    {
        
    }

    void OnEnable()
    {
        StartCoroutine(GO());
    }

    private IEnumerator GO()
    {
        while (blackScreen.color.a < 1)
        {
            blackScreen.color += new Color(0, 0, 0, Time.deltaTime / 2);
            yield return null;
        }
        while (text.color.a < 1)
        {
            text.color += new Color(0, 0, 0, Time.deltaTime / 2);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        options.SetActive(true);
    }

}
