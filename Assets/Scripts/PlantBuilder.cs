using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public List<int> assignedPlants;
    /// <summary> The currently selected plant object that, if clicked on a tile, will be planted </summary>
    public static GameObject currentPlant;

    /// <summary> The global amount of sun the player currently has </summary>
    public static int sun;

    private TextMeshProUGUI s;

    // Start is called before the first frame update
    void Awake()
    {
        assignedPlants = new List<int>(new int[] {33, 1, 20, allPlants.Length - 1, 14, 5, 25, 27});
    }

    void Start()
    {
        s = transform.Find("Sun").Find("Text").GetComponent<TextMeshProUGUI>();
        currentPlant = allPlants[assignedPlants[0]];
        sun = FindFirstObjectByType<Level>().startingSun;
    }

    void Update()
    {
        s.text = sun + "";
    }

    public void SetPlantToBuild(int buttonID)
    {
        if (buttonID >= assignedPlants.Count) return;
        currentPlant = allPlants[assignedPlants[buttonID]];
    }

}
