using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shovel : MonoBehaviour
{

    public AudioClip select;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Shovel")) OnClick();
    }

    public void OnClick()
    {
        if (Time.timeScale == 0) return;
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }
        SFX.Instance.Play(select);
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

}
