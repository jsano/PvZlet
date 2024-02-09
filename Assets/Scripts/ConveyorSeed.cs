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
    private Rigidbody2D RB;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        RT = GetComponent<RectTransform>();
        RB = GetComponent<Rigidbody2D>();
        RB.velocity = Vector3.up * 100;
        pb = GameObject.Find("PlantBuilder").GetComponent<PlantBuilder>();
        image = GetComponent<Image>();
        image.color = pb.allPlants[plant].GetComponent<SpriteRenderer>().color + new Color(0, 0, 0, 1);
        //image.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //image.fillAmount += 100 * Time.deltaTime / RT.sizeDelta.y;
        if (Physics2D.Raycast(RT.position + new Vector3(0, RT.sizeDelta.y, 0), Vector2.up, 5f, LayerMask.GetMask("Seed"))) RB.velocity = Vector3.zero;
        else RB.velocity = Vector2.up * 100;
        if (RT.anchoredPosition.y >= 0)
        RB.velocity = Vector3.zero;
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
