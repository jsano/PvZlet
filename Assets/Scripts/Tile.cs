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

    /// <summary> The plant object currently planted onto this tile. Can be null if the tile is empty </summary>
    public GameObject placed;

    // Start is called before the first frame update
    void Start()
    {
        ZombieSpawner.ROW_TO_WORLD[row] = transform.position.y;
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

}
