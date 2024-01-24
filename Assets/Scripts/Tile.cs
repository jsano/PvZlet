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

    /// <summary> The global mapping between row number to world y-coordinates. Length is 7 with index 0 as a buffer and index 6 never used if there's only 5 lanes </summary>
    public static float[] ROW_TO_WORLD = new float[7];
    /// <summary> The global mapping between column number to world x-coordinates. Length is 10 with index 0 as a buffer </summary>
    public static float[] COL_TO_WORLD = new float[10];

    public static Tile[,] tileObjects = new Tile[7,10];

    /// <summary> The plant object currently planted onto this tile. Can be null if there's no plant </summary>
    public GameObject planted;
    /// <summary> The grid item currently placed onto this tile. Can be null if the tile is empty </summary>
    public GameObject gridItem;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>().lanes == 6) TILE_DISTANCE = new Vector2(2, 2.24f);
        else TILE_DISTANCE = new Vector2(2, 2.4f);
        ROW_TO_WORLD[row] = transform.position.y;
        COL_TO_WORLD[col] = transform.position.x;
        SR = GetComponent<SpriteRenderer>();
        tileObjects[row, col] = this;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    // NOTE: not sure when I'd use this when I can just use COL_TO_WORLD?
    /// <summary> Given a x-position in world units, return the closest corresponding in-game column it belongs to </summary>
    /// <param name="x"> The x-position of the object </param>
    /// <returns> The column number the object is in, between 1-9. Can return 0 if it's off the lawn </returns>
    public static int WORLD_TO_COL(float x)
    {
        for (int i = 1; i < COL_TO_WORLD.Length; i++)
        {
            if (Mathf.Abs(COL_TO_WORLD[i] - x) <= TILE_DISTANCE.x / 2) return i;
        }
        return 0;
    }

    /// <summary> Put a plant or grid item into this tile. If it's a grid item, overwrites any previous plant </summary>
    /// <param name="toPlace"> The game object to place </param>
    public void Place(GameObject toPlace)
    {
        GameObject g = Instantiate(toPlace, transform.position, Quaternion.identity);
        Plant p = g.GetComponent<Plant>();
        if (p != null)
        {
            planted = g;
            p.row = row;
            p.col = col;
            PlantBuilder.sun -= p.cost;
        }
        else
        {
            Destroy(planted);
            gridItem = g;
        }
    }

    private bool CanPlantHere()
    {
        GameObject p = PlantBuilder.currentPlant;
        if (p.GetComponent<GraveBuster>() != null) {
            if (planted == null && gridItem != null && gridItem.tag == "Grave") return true;
            return false;
        }
        if (water && !p.GetComponent<Plant>().aquatic) return false; //TODO: replace
        /*if (p.GetComponent<LilyPad>() != null)
        {
            if (planted == null && gridItem != null && gridItem.tag == "Grave") return true;
            return false;
        }*/
        if (planted == null && gridItem == null) return true;
        return false;
    }

}
