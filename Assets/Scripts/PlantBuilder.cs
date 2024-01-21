using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    private static GameObject currentPlant;

    /// <summary> How much sun the level starts with </summary>
    public int startingSun;
    /// <summary> The global amount of sun the player currently has </summary>
    public static int sun;

    // Start is called before the first frame update
    void Awake()
    {
        assignedPlants = new int[] {0, 1, 2, 3, 4, 5, 6, 7};
        sun = startingSun;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            if (raycastResults.Count == 0) EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void SetPlantToBuild(int buttonID)
    {
        if (buttonID >= assignedPlants.Length) return;
        currentPlant = allPlants[assignedPlants[buttonID]];
    }

    public static void Plant(Tile t)
    {
        GameObject g = Instantiate(currentPlant, t.transform.position, Quaternion.identity);
        if (!g.GetComponent<Plant>().instant) t.placed = g;
        g.GetComponent<Plant>().row = t.row;
        sun -= g.GetComponent<Plant>().cost;
    }

}
