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

    public void SetPlantToBuild(int buttonID)
    {
        if (buttonID == planting) planting = -1;
        else planting = buttonID;
        Debug.Log(assignedPlants.Length);
        currentPlant = allPlants[assignedPlants[buttonID]];
    }

    public static void Plant(Vector3 pos)
    {
        Instantiate(currentPlant, pos, Quaternion.identity);
        planting = -1;
    }

}
