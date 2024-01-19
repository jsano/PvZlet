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

    public GameObject[] allPlants;

    public int[] assignedPlants;
    private static GameObject currentPlant;

    public int startingSun;
    public static int sun;

    // Start is called before the first frame update
    void Awake()
    {
        assignedPlants = new int[] {0, 1, 2, 3};
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
