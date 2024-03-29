using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetting : MonoBehaviour
{

    private Slider s;
    public string key;

    void Awake()
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
        if (s != null) PlayerPrefs.SetFloat(key, s.value);
    }

}
