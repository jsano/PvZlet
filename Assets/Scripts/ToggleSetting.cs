using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSetting : MonoBehaviour
{

    private Toggle s;
    public string key;

    void Awake()
    {
        s = GetComponent<Toggle>();
        s.isOn = PlayerPrefs.GetInt(key, 0) == 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChanged()
    {
        if (s != null) PlayerPrefs.SetInt(key, s.isOn ? 1 : 0);
    }

}
