using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{

    public GameObject conveyorSeed;
    private Level l;
    private float interval = 5;
    private float period;
    [HideInInspector] public int count;

    // Start is called before the first frame update
    void Start()
    {
        period = interval / 2;
        l = FindFirstObjectByType<Level>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.status != LevelManager.Status.Start) return;
        if (count < 8) period += Time.deltaTime;
        if (period >= interval)
        {
            period = 0;
            int range = 0;
            foreach (Level.Data d in l.conveyor) range += Mathf.Max(0, d.count - PlantBuilder.plantCounts[d.plant]);
            if (range == 0) return;
            int index = Random.Range(0, range);
            int plant = 0;
            foreach (Level.Data d in l.conveyor)
            {
                index -= d.count - PlantBuilder.plantCounts[d.plant];
                if (index < 0)
                {
                    plant = d.plant;
                    break;
                }
            }
            GameObject g = Instantiate(conveyorSeed, transform);
            float pos = -(GetComponent<RectTransform>().sizeDelta.y - g.GetComponent<RectTransform>().sizeDelta.y);
            g.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, pos);
            
            g.GetComponent<ConveyorSeed>().plant = plant;
            PlantBuilder.plantCounts[plant] += 1;
            count += 1;
        }
    }

}
