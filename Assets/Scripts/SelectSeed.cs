using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSeed : MonoBehaviour
{

    public int ID;
    private Plant plant;
    private bool chosen;

    public AudioClip choose;
    public AudioClip unchoose;
    public AudioClip banned;

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
        if (!GetComponent<Button>().interactable)
        {
            SFX.Instance.Play(banned);
            return;
        }
        if (!chosen)
        {
            if (PlantBuilder.Instance.assignedPlants.Count == 8 || PlantBuilder.Instance.assignedPlants.Contains(ID)) return;
            SFX.Instance.Play(choose);
            PlantBuilder.Instance.assignedPlants.Add(ID);
            GetComponent<Image>().color -= new Color(0, 0, 0, 0.5f);
        }
        else
        {
            PlantBuilder.Instance.assignedPlants.RemoveAt(PlantBuilder.Instance.assignedPlants.IndexOf(ID));
            SFX.Instance.Play(unchoose);
            GetComponent<Image>().color += new Color(0, 0, 0, 0.5f);
        }
        chosen = !chosen;
    }

}
