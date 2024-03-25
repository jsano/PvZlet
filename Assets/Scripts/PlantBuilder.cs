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

    [HideInInspector] public static int[] plantCounts;

    /// <summary> The currently picked plants for the level, represented by indices for <c>allPlants</c> </summary>
    [HideInInspector] public List<int> assignedPlants;
    /// <summary> The currently selected plant object that, if clicked on a tile, will be planted </summary>
    public static GameObject currentPlant;

    /// <summary> The global amount of sun the player currently has </summary>
    public static int sun;

    public GameObject BG;
    public GameObject selectSeed;
    [HideInInspector] public List<GameObject> selectSeeds = new List<GameObject>();
    private TextMeshProUGUI s;
    private ZombieSpawner ZS;

    void Start()
    {
        s = transform.Find("Sun").Find("Text").GetComponent<TextMeshProUGUI>();
        Level l = FindFirstObjectByType<Level>();
        currentPlant = allPlants[0];
        plantCounts = new int[allPlants.Length];
        for (int i = 0; i < Mathf.Min(l.unlockedUntil, allPlants.Length); i++)
        {
            GameObject g = Instantiate(selectSeed, BG.transform);
            g.GetComponent<SelectSeed>().ID = i;
            selectSeeds.Add(g);
        }

        ZS = GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>();
        if (l.potColumn > 0) {
            for (int i = 1; i <= ZS.lanes; i++)
                for (int j = 1; j <= l.potColumn; j++) Tile.tileObjects[i, j].Place(allPlants[33]);
        }
        
        sun = l.startingSun;
    }

    void Update()
    {
        s.text = sun + "";
    }

    public void SetPlantToBuild(int buttonID)
    {
        if (buttonID >= assignedPlants.Count) return;
        currentPlant = allPlants[assignedPlants[buttonID]];
        currentPlant.GetComponent<Plant>().ID = assignedPlants[buttonID];
    }

    public void SetPlantIDToBuild(int plantID)
    {
        if (plantID >= allPlants.Length) return;
        currentPlant = allPlants[plantID];
        currentPlant.GetComponent<Plant>().ID = plantID;
    }

}
