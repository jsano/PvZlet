using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlmanacSelectSeed : MonoBehaviour
{

    public int ID;
    public bool isPlant;
    private Plant plant;
    private Zombie zombie;

    public AudioClip select;

    // Start is called before the first frame update
    void Start()
    {
        if (isPlant)
        {
            plant = PlantBuilder.Instance.allPlants[ID].GetComponent<Plant>();
            transform.Find("Plant").GetComponent<Image>().sprite = plant.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            zombie = ZombieSpawner.Instance.allZombies[ID].GetComponent<Zombie>();
            transform.Find("Plant").GetComponent<Image>().sprite = zombie.GetComponent<SpriteRenderer>().sprite;
            transform.Find("Plant").GetComponent<Image>().color = zombie.GetComponent<SpriteRenderer>().color;
        }
    }

    public void OnClick()
    {
        Almanac.Instance.Show(ID);
    }

}
