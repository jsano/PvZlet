using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBuilder : MonoBehaviour
{

    /*public static PlantBuilder instance;

    void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }*/

    public GameObject[] allPlants;

    private int[] assignedPlants;
    private static GameObject currentPlant;
    [HideInInspector] public static int planting = -1;

    // Start is called before the first frame update
    void Start()
    {
        assignedPlants = new int[] {2, 0, 1};
    }

    void Update()
    {
        if (Input.GetButtonDown("Plant1")) SetPlantToBuild(0);
        if (Input.GetButtonDown("Plant2")) SetPlantToBuild(1);
        if (Input.GetButtonDown("Plant3")) SetPlantToBuild(2);
        if (Input.GetButtonDown("Plant4")) SetPlantToBuild(3);
        if (Input.GetButtonDown("Plant5")) SetPlantToBuild(4);
        if (Input.GetButtonDown("Plant6")) SetPlantToBuild(5);
        if (Input.GetButtonDown("Plant7")) SetPlantToBuild(6);
        if (Input.GetButtonDown("Plant8")) SetPlantToBuild(7);
        if (Input.GetButtonDown("Plant9")) SetPlantToBuild(8);
    }

    public void SetPlantToBuild(int buttonID)
    {
        if (buttonID >= assignedPlants.Length) return;
        if (buttonID == planting) planting = -1;
        else planting = buttonID;
        currentPlant = allPlants[assignedPlants[buttonID]];
    }

    public static void Plant(Tile t)
    {
        GameObject g = Instantiate(currentPlant, t.transform.position, Quaternion.identity);
        t.placed = g;
        planting = -1;
    }

}
