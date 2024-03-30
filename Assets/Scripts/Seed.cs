using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Seed : SeedBase
{

    private PlantBuilder pb;
    /// <summary> The ID of the button, used as the index for the PlantBuilder's <c>assignedPlants</c>. 0-indexed </summary>
    public int ID;
    private Button b;
    private Plant plant;
    private Image recharge;
    private float rechargePeriod;
    private TextMeshProUGUI cost;
    private Image image;
    private Image plantImage;

    public AudioClip unchoose;
    public AudioClip select;
    public AudioClip notReady;

    // Start is called before the first frame update
    void Start()
    {
        pb = GameObject.Find("PlantBuilder").GetComponent<PlantBuilder>();
        b = GetComponent<Button>();
        cost = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        recharge = transform.Find("Recharge").GetComponent<Image>();
        recharge.fillAmount = 0;
        image = GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0.5f);
        plantImage = transform.Find("Plant").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.status == LevelManager.Status.Intro)
        {
            if (pb.assignedPlants.Count <= ID)
            {
                image.color = new Color(0, 0, 0, 0.5f);
                cost.text = "";
                plant = null;
                plantImage.color = Color.clear;
                return;
            }
            plant = pb.allPlants[pb.assignedPlants[ID]].GetComponent<Plant>();
            rechargePeriod = plant.recharge;
            cost.text = plant.cost + "";
            image.color = Color.white;
            plantImage.sprite = plant.GetComponent<SpriteRenderer>().sprite;
            plantImage.color = Color.white;
        }
        else
        {
            if (plant == null) return;
            rechargePeriod += Time.deltaTime;
            b.interactable = PlantBuilder.sun >= plant.cost && rechargePeriod >= plant.recharge;
            recharge.fillAmount = (rechargePeriod / plant.recharge >= 1) ? 0 : rechargePeriod / plant.recharge;

            if (Input.GetButtonDown("Plant1") && ID == 0) OnClick();
            if (Input.GetButtonDown("Plant2") && ID == 1) OnClick();
            if (Input.GetButtonDown("Plant3") && ID == 2) OnClick();
            if (Input.GetButtonDown("Plant4") && ID == 3) OnClick();
            if (Input.GetButtonDown("Plant5") && ID == 4) OnClick();
            if (Input.GetButtonDown("Plant6") && ID == 5) OnClick();
            if (Input.GetButtonDown("Plant7") && ID == 6) OnClick();
            if (Input.GetButtonDown("Plant8") && ID == 7) OnClick();
        }
        /*
        //DEBUG
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            PlantBuilder.sun = 1000;
            rechargePeriod = plant.recharge;
        }*/
    }

    /// <summary> Called when the button is clicked or the hotkey is pressed </summary>
    public override void OnClick()
    {
        if (Time.timeScale == 0) return;
        if (!b.interactable)
        {
            SFX.Instance.Play(notReady);
            return;
        }
        if (LevelManager.status == LevelManager.Status.Intro)
        {
            if (pb.assignedPlants.Count > ID)
            {
                pb.selectSeeds[pb.assignedPlants[ID]].GetComponent<Button>().interactable = true;
                pb.assignedPlants.RemoveAt(ID);
                SFX.Instance.Play(unchoose);
            }
            return;
        }
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }
        SFX.Instance.Play(select);
        EventSystem.current.SetSelectedGameObject(gameObject);
        pb.SetPlantToBuild(ID);
    }

    public override void OnPlant()
    {
        rechargePeriod = 0;
        PlantBuilder.plantCounts[pb.assignedPlants[ID]] += 1;
    }

}
