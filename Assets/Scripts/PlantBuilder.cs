using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    //NOTE: MAYBE MAKE THESE ALL STATIC

    public GameObject[] allPlants;

    public int[] assignedPlants;
    private static GameObject currentPlant;
    [HideInInspector] public static int planting = -1;

    public int sun = 100;

    // Start is called before the first frame update
    void Awake()
    {
        assignedPlants = new int[] {2, 0, 1};
    }

    void Update()
    {

    }

    public void SetPlantToBuild(int buttonID)
    {
        if (buttonID >= assignedPlants.Length) return;
        if (buttonID == planting)
        {
            planting = -1;
            EventSystem.current.SetSelectedGameObject(null);
        }
        else planting = buttonID;
        currentPlant = allPlants[assignedPlants[buttonID]];
    }

    public static void Plant(Tile t)
    {
        GameObject g = Instantiate(currentPlant, t.transform.position, Quaternion.identity);
        t.placed = g;
        g.GetComponent<Plant>().row = t.row;
        planting = -1;
        //sun -= g.GetComponent<Plant>().cost;
    }

}
