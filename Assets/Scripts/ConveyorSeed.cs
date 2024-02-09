using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConveyorSeed : SeedBase
{

    private PlantBuilder pb;
    [HideInInspector] public int plant;
    private RectTransform RT;

    // Start is called before the first frame update
    void Start()
    {
        RT = GetComponent<RectTransform>();
        pb = GameObject.Find("PlantBuilder").GetComponent<PlantBuilder>();
        GetComponent<Image>().color = pb.allPlants[plant].GetComponent<SpriteRenderer>().color + new Color(0, 0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (RT.anchoredPosition.y < 0) RT.anchoredPosition += Vector2.up * 100 * Time.deltaTime;
    }

    /// <summary> Called when the button is clicked or the hotkey is pressed </summary>
    public override void OnClick()
    {
        if (Time.timeScale == 0) return;
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }
        EventSystem.current.SetSelectedGameObject(gameObject);
        pb.SetPlantIDToBuild(plant);
    }

    public override void OnPlant()
    {
        Destroy(gameObject);
    }

}
