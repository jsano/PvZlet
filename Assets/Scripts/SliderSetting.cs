using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetting : MonoBehaviour
{

    private Slider s;
    public string key;

    // Start is called before the first frame update
    void Start()
    {
        s = GetComponent<Slider>();
        s.value = PlayerPrefs.GetFloat(key, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChanged()
    {
        PlayerPrefs.SetFloat(key, s.value);
    }

}
