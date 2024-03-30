using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSeed : MonoBehaviour
{

    public int ID;
    private Plant plant;

    public AudioClip choose;

    // Start is called before the first frame update
    void Start()
    {
        plant = PlantBuilder.Instance.allPlants[ID].GetComponent<Plant>();
        transform.Find("Text").GetComponent<TextMeshProUGUI>().text = plant.cost + "";
        transform.Find("Plant").GetComponent<Image>().sprite = plant.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (PlantBuilder.Instance.assignedPlants.Count == 8 || PlantBuilder.Instance.assignedPlants.Contains(ID)) return;
        SFX.Instance.Play(choose);
        PlantBuilder.Instance.assignedPlants.Add(ID);
        GetComponent<Button>().interactable = false;
    }

}
