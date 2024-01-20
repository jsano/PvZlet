using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{

    public Color hoverColor;
    private SpriteRenderer SR;

    public int row;
    public int col;

    public static readonly Vector2 TILE_DISTANCE = new Vector2(2, 2.4f);

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
