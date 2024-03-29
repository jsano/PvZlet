using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSeed : MonoBehaviour
{

    public int ID;
    private PlantBuilder pb;
    private Plant plant;

    public AudioClip choose;

    // Start is called before the first frame update
    void Start()
    {
        pb = GameObject.Find("PlantBuilder").GetComponent<PlantBuilder>();
        plant = pb.allPlants[ID].GetComponent<Plant>();
        transform.Find("Text").GetComponent<TextMeshProUGUI>().text = plant.cost + "";
        //GetComponent<Image>().color = plant.GetComponent<SpriteRenderer>().color + new Color(0, 0, 0, 1);
        transform.Find("Plant").GetComponent<Image>().sprite = plant.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (pb.assignedPlants.Count == 8 || pb.assignedPlants.Contains(ID)) return;
        SFX.Instance.Play(choose);
        pb.assignedPlants.Add(ID);
        GetComponent<Button>().interactable = false;
    }

}
