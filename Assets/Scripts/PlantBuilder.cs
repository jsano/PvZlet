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
    //NOTE: MAYBE MAKE THESE ALL STATIC

    /// <summary> The master list of all plants in the game </summary>
    public GameObject[] allPlants;

    /// <summary> The currently picked plants for the level, represented by indices for <c>allPlants</c> </summary>
    public int[] assignedPlants;
    /// <summary> The currently selected plant object that, if clicked on a tile, will be planted </summary>
    public static GameObject currentPlant;

    /// <summary> The global amount of sun the player currently has </summary>
    public static int sun;

    // Start is called before the first frame update
    void Awake()
    {
        assignedPlants = new int[] {33, 1, 20, allPlants.Length - 1, 14, 5, 31, 21};
    }

    void Start()
    {
        currentPlant = allPlants[assignedPlants[0]];
        sun = FindFirstObjectByType<Level>().startingSun;
    }

    void Update()
    {
        
    }

    public void SetPlantToBuild(int buttonID)
    {
        if (buttonID >= assignedPlants.Length) return;
        currentPlant = allPlants[assignedPlants[buttonID]];
    }

}
