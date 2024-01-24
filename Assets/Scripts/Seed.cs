using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Seed : MonoBehaviour
{

    private PlantBuilder pb;
    /// <summary> The ID of the button, used as the index for the PlantBuilder's <c>assignedPlants</c>. 0-indexed </summary>
    public int ID;
    private Button b;
    private Plant plant;

    // Start is called before the first frame update
    void Start()
    {
        pb = GameObject.Find("PlantBuilder").GetComponent<PlantBuilder>();
        b = GetComponent<Button>();
        plant = pb.allPlants[pb.assignedPlants[ID]].GetComponent<Plant>();
        transform.Find("Text").GetComponent<TextMeshProUGUI>().text = plant.cost + "";
        GetComponent<Image>().color = plant.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        b.interactable = PlantBuilder.sun >= plant.cost;

        if (Input.GetButtonDown("Plant1") && ID == 0 && b.interactable) OnClick();
        if (Input.GetButtonDown("Plant2") && ID == 1 && b.interactable) OnClick();
        if (Input.GetButtonDown("Plant3") && ID == 2 && b.interactable) OnClick();
        if (Input.GetButtonDown("Plant4") && ID == 3 && b.interactable) OnClick();
        if (Input.GetButtonDown("Plant5") && ID == 4 && b.interactable) OnClick();
        if (Input.GetButtonDown("Plant6") && ID == 5 && b.interactable) OnClick();
        if (Input.GetButtonDown("Plant7") && ID == 6 && b.interactable) OnClick();
        if (Input.GetButtonDown("Plant8") && ID == 7 && b.interactable) OnClick();
    }

    /// <summary> Called when the button is clicked or the hotkey is pressed </summary>
    public void OnClick()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }
        EventSystem.current.SetSelectedGameObject(gameObject);
        pb.SetPlantToBuild(ID);
    }

}
