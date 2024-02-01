using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{

    /// <summary> The color to display when the mouse is over the tile and about to do something </summary>
    public Color hoverColor;
    private SpriteRenderer SR;

    /// <summary> Which row the tile is in. Takes values between [1 - <c>ZombieSpawner.lanes</c>] </summary>
    public int row;
    /// <summary> Which column the tile is in. Takes values between [1 - 9] </summary>
    public int col;
    public bool water;
    public bool roof;

    /// <summary> The global distance in world units that a tile takes up </summary>
    public static Vector2 TILE_DISTANCE;

    /// <summary> The global mapping between column number to world x-coordinates. Length is 10 with index 0 as a buffer </summary>
    public static float[] COL_TO_WORLD = new float[10];

    public static Tile[,] tileObjects = new Tile[7,10];

    /// <summary> The plant object currently planted onto this tile. Can be null if there's no plant </summary>
    [HideInInspector] public GameObject planted;
    /// <summary> The grid item currently placed onto this tile. Can be null if the tile is empty </summary>
    [HideInInspector] public GameObject gridItem;
    /// <summary> Any extra plants that are part of a combined plant, in order of importance (ex. pumpkin < lilypad) </summary>
    private List<GameObject> overlapped = new List<GameObject>();
    [HideInInspector] public GameObject ladder;
    /// <summary> The fog currently covering this tile. Can be null if there's no fog </summary>
    [HideInInspector] public GameObject fog;

    // Start is called before the first frame update
    void Start()
    {
        TILE_DISTANCE = new Vector2(transform.localScale.x + 0.1f, transform.localScale.y + 0.1f);
        COL_TO_WORLD[col] = transform.position.x;
        SR = GetComponent<SpriteRenderer>();
        tileObjects[row, col] = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (planted == null && overlapped.Count > 0)
        {
            planted = overlapped[0];
            overlapped.RemoveAt(0);
        }
    }

    void OnMouseOver()
    {
        if (EventSystem.current.currentSelectedGameObject != null && CanPlantHere()) SR.color = hoverColor;
        else SR.color = Color.clear;
    }

    void OnMouseExit()
    {
        SR.color = Color.clear;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.currentSelectedGameObject != null && CanPlantHere()) Place(PlantBuilder.currentPlant);
    }

    /// <summary> Given a x-position in world units, return the closest corresponding in-game column it belongs to </summary>
    /// <param name="x"> The x-position of the object </param>
    /// <returns> The column number the object is in, between 1-9. Can return 0 if it's off the lawn </returns>
    public static int WORLD_TO_COL(float x)
    {
        if (COL_TO_WORLD[1] - x > TILE_DISTANCE.x / 2) return 0;
        for (int i = 1; i < COL_TO_WORLD.Length; i++)
        {
            if (Mathf.Abs(COL_TO_WORLD[i] - x) <= TILE_DISTANCE.x / 2) return i;
        }
        return 10;
    }

    /// <summary> Put a plant or grid item into this tile. If it's a grid item, overwrites any previous plant </summary>
    /// <param name="toPlace"> The game object to place </param>
    public void Place(GameObject toPlace)
    {
        GameObject g = Instantiate(toPlace, transform.position, Quaternion.identity);
        Plant p = g.GetComponent<Plant>();
        if (p != null)
        {
            if (planted != null)
            {
                if (g.tag == "Pumpkin" && planted.tag != "LilyPad") overlapped.Insert(0, g);
                else if (g.tag != "CoffeeBean")
                {
                    overlapped.Add(planted);
                    planted = g;
                }
            }
            else planted = g;
            p.row = row;
            p.col = col;
            PlantBuilder.sun -= p.cost;
        }
        else
        {
            RemoveAllPlants();
            Destroy(gridItem);
            gridItem = g;
        }
    }

    private bool CanPlantHere()
    {
        Plant p = PlantBuilder.currentPlant.GetComponent<Plant>();
        if (gridItem != null)
        {
            if (p.GetComponent<GraveBuster>() != null && gridItem.tag == "Grave") return true;
            return false;
        }
        if (p.tag == "Pumpkin" && (!water && !roof || planted != null) && ContainsPlant("Pumpkin") == null) return true;
        if (p.tag == "FlowerPot" && !roof) return false;
        if (p.tag == "CoffeeBean") return planted != null && planted.GetComponent<Plant>().isSleeping();
        if (p.grounded && (water || roof)) return false;
        if (water)
        {
            if (p.aquatic && planted == null) return true;
            if (!p.aquatic && ContainsPlant("LilyPad") == null) return false;
        }
        if (roof)
        {
            if (p.tag == "FlowerPot" && planted == null) return true;
            if (p.tag != "FlowerPot" && ContainsPlant("FlowerPot") == null) return false;
        }
        if (p.aquatic && !water) return false;
        if (planted == null || planted.tag == "Pumpkin" || planted.tag == "LilyPad" || planted.tag == "FlowerPot") return true;
        return false;
    }

    public GameObject ContainsPlant(string s)
    {
        if (planted != null && planted.name.StartsWith(s)) return planted;
        foreach (GameObject g in overlapped) if (g.name.StartsWith(s)) return g;
        return null;
    }

    public GameObject GetEatablePlant()
    {
        if (planted == null) return null;
        GameObject p = ContainsPlant("Pumpkin");
        if (p != null) return p;
        if (planted.GetComponent<Plant>().isActiveInstant() && overlapped.Count > 0) return overlapped[0];
        return planted;
    }

    public void RemoveAllPlants(GameObject source = null)
    {
        Destroy(ladder);
        foreach (GameObject g in overlapped) g.GetComponent<Plant>().ReceiveDamage(1000, source);
        overlapped.Clear();
        if (planted != null && !planted.GetComponent<Plant>().isActiveInstant()) planted.GetComponent<Plant>().ReceiveDamage(1000, source);
    }

}
