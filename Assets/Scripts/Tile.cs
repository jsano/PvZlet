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

    /// <summary> The global distance in world units that a tile takes up </summary>
    public static readonly Vector2 TILE_DISTANCE = new Vector2(2, 2.4f);

    /// <summary> The global mapping between row number to world y-coordinates. Length is 7 with index 0 as a buffer and index 6 never used if there's only 5 lanes </summary>
    public static float[] ROW_TO_WORLD = new float[6];
    /// <summary> The global mapping between column number to world x-coordinates. Length is 10 with index 0 as a buffer </summary>
    public static float[] COL_TO_WORLD = new float[10];

    /// <summary> The plant object currently planted onto this tile. Can be null if the tile is empty </summary>
    public GameObject placed;

    // Start is called before the first frame update
    void Start()
    {
        ROW_TO_WORLD[row] = transform.position.y;
        COL_TO_WORLD[col] = transform.position.x;
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        if (EventSystem.current.currentSelectedGameObject != null && placed == null) SR.color = hoverColor;
        else SR.color = Color.clear;
    }

    void OnMouseExit()
    {
        SR.color = Color.clear;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.currentSelectedGameObject != null && placed == null) PlantBuilder.Plant(this);
    }

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

}
