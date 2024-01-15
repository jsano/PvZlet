using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public Color hoverColor;
    private SpriteRenderer SR;

    public static readonly Vector2 TILE_DISTANCE = new Vector2(2, 2.4f);

    public GameObject placed;

    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        if (PlantBuilder.planting != -1 && placed == null) SR.color = hoverColor;
    }

    void OnMouseExit()
    {
        SR.color = Color.clear;
    }

    void OnMouseDown()
    {
        if (PlantBuilder.planting != -1 && placed == null) PlantBuilder.Plant(this);
    }

}
