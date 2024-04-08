using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantBuilder : MonoBehaviour
{

    private static PlantBuilder instance;
    public static PlantBuilder Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
    }

    /// <summary> The master list of all plants in the game </summary>
    public GameObject[] allPlants;

    [HideInInspector] public int[] plantCounts;

    /// <summary> The currently picked plants for the level, represented by indices for <c>allPlants</c> </summary>
    [HideInInspector] public List<int> assignedPlants;
    /// <summary> The currently selected plant object that, if clicked on a tile, will be planted </summary>
    public GameObject currentPlant;

    /// <summary> The global amount of sun the player currently has </summary>
    public int sun;

    public GameObject BG;
    public GameObject selectSeed;
    [HideInInspector] public List<GameObject> selectSeeds = new List<GameObject>();
    private TextMeshProUGUI s;

    void Start()
    {
        s = transform.Find("Sun").Find("Text").GetComponent<TextMeshProUGUI>();
        currentPlant = allPlants[0];
        plantCounts = new int[allPlants.Length];
        for (int i = 0; i < Mathf.Min(Level.currentLevel.unlockedUntil, allPlants.Length); i++)
        {
            GameObject g = Instantiate(selectSeed, BG.transform);
            g.GetComponent<SelectSeed>().ID = i;
            selectSeeds.Add(g);
            if (Level.currentLevel.banned.Contains(i)) g.GetComponent<Button>().interactable = false;
        }

        if (Level.currentLevel.potColumn > 0) {
            for (int i = 1; i <= ZombieSpawner.Instance.lanes; i++)
                for (int j = 1; j <= Level.currentLevel.potColumn; j++)
                {
                    Tile.tileObjects[i, j].Place(allPlants[33]);
                    plantCounts[33] += 1;
                }
        }
        
        sun = Level.currentLevel.startingSun;
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
