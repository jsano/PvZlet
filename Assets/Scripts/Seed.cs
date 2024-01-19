using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Seed : MonoBehaviour
{

    private PlantBuilder pb;
    public int ID;
    private Button b;
    private Plant plant;

    // Start is called before the first frame update
    void Start()
    {
        pb = GameObject.Find("PlantBuilder").GetComponent<PlantBuilder>();
        b = GetComponent<Button>();
        plant = pb.allPlants[pb.assignedPlants[ID]].GetComponent<Plant>();
    }

    // Update is called once per frame
    void Update()
    {
        b.interactable = pb.sun >= plant.cost;

        if (Input.GetButtonDown("Plant1") && ID == 0 && b.enabled) OnClick();
        if (Input.GetButtonDown("Plant2") && ID == 1 && b.enabled) OnClick();
        if (Input.GetButtonDown("Plant3") && ID == 2 && b.enabled) OnClick();
        if (Input.GetButtonDown("Plant4") && ID == 3 && b.enabled) OnClick();
        if (Input.GetButtonDown("Plant5") && ID == 4 && b.enabled) OnClick();
        if (Input.GetButtonDown("Plant6") && ID == 5 && b.enabled) OnClick();
        if (Input.GetButtonDown("Plant7") && ID == 6 && b.enabled) OnClick();
        if (Input.GetButtonDown("Plant8") && ID == 7 && b.enabled) OnClick();
        if (Input.GetButtonDown("Plant9") && ID == 8 && b.enabled) OnClick();
    }

    public void OnClick()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        pb.SetPlantToBuild(ID);
    }

}
