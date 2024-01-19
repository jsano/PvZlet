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

    public int startingSun;
    public static int sun;

    // Start is called before the first frame update
    void Awake()
    {
        assignedPlants = new int[] {2, 0, 1};
        sun = startingSun;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && EventSystem.current.currentSelectedGameObject != null) 
            EventSystem.current.SetSelectedGameObject(null);
    }

    public void SetPlantToBuild(int buttonID)
    {
        if (buttonID >= assignedPlants.Length) return;
        currentPlant = allPlants[assignedPlants[buttonID]];
    }

    public static void Plant(Tile t)
    {
        GameObject g = Instantiate(currentPlant, t.transform.position, Quaternion.identity);
        t.placed = g;
        g.GetComponent<Plant>().row = t.row;
        sun -= g.GetComponent<Plant>().cost;
    }

}
