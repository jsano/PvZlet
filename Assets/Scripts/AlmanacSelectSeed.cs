using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlmanacSelectSeed : MonoBehaviour
{

    public int ID;
    public bool isPlant;
    private Plant plant;
    private Zombie zombie;

    public AudioClip select;

    public void afterStart()
    {
        if (isPlant)
        {
            plant = PlantBuilder.Instance.allPlants[ID].GetComponent<Plant>();
            transform.Find("Plant").GetComponent<Image>().sprite = plant.GetComponent<SpriteRenderer>().sprite;
            transform.Find("Text").GetComponent<TextMeshProUGUI>().text = PlantBuilder.Instance.allPlants[ID].GetComponent<Plant>().cost + "";
        }
        else
        {
            zombie = ZombieSpawner.Instance.allZombies[ID].GetComponent<Zombie>();
            transform.Find("Plant").GetComponent<Image>().sprite = ZombieSpawner.Instance.allZombies[0].GetComponent<SpriteRenderer>().sprite;
            transform.Find("Plant").GetComponent<Image>().color = ZombieSpawner.Instance.allZombies[ID].GetComponent<SpriteRenderer>().color;
        }
    }

    public void OnClick()
    {
        Almanac.Instance.Show(ID);
    }

}
