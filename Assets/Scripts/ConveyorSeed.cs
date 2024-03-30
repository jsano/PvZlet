using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConveyorSeed : SeedBase
{

    [HideInInspector] public int plant;
    private RectTransform RT;
    private Image image;

    private float heightScale;

    public AudioClip select;

    // Start is called before the first frame update
    void Start()
    {
        RT = GetComponent<RectTransform>();
        heightScale = PlantBuilder.Instance.GetComponent<RectTransform>().rect.size.y / RT.rect.size.y;
        image = GetComponent<Image>();
        image.color = PlantBuilder.Instance.allPlants[plant].GetComponent<SpriteRenderer>().color + new Color(0, 0, 0, 1);
        //image.fillAmount = 0;
        transform.Find("Plant").GetComponent<Image>().sprite = PlantBuilder.Instance.allPlants[plant].GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        //image.fillAmount += 100 * Time.deltaTime / RT.sizeDelta.y;
        if (RT.anchoredPosition.y < 0 && Physics2D.RaycastAll(RT.position, Vector2.up, Screen.height / heightScale, LayerMask.GetMask("Seed")).Length <= 1)
            RT.anchoredPosition = RT.anchoredPosition + Vector2.up * 100 * Time.deltaTime;
    }

    /// <summary> Called when the button is clicked or the hotkey is pressed </summary>
    public override void OnClick()
    {
        if (Time.timeScale == 0) return;
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }
        SFX.Instance.Play(select);
        EventSystem.current.SetSelectedGameObject(gameObject);
        PlantBuilder.Instance.SetPlantIDToBuild(plant);
    }

    public override void OnPlant()
    {
        Destroy(gameObject);
        transform.parent.GetComponent<Conveyor>().count -= 1;
    }

}
