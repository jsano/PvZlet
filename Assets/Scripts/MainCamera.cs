using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCamera : MonoBehaviour
{

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.eventMask = LayerMask.GetMask("UI");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
