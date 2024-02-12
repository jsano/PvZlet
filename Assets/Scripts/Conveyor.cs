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
            List<Level.Data> options = new List<Level.Data>();
            foreach (Level.Data d in l.conveyor) if (d.count > PlantBuilder.plantCounts[d.plant]) options.Add(d);
            if (options.Count == 0) return;
            GameObject g = Instantiate(conveyorSeed, transform);
            float pos = -(GetComponent<RectTransform>().sizeDelta.y - g.GetComponent<RectTransform>().sizeDelta.y);
            g.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, pos);
            int index = Random.Range(0, options.Count);
            g.GetComponent<ConveyorSeed>().plant = options[index].plant;
            PlantBuilder.plantCounts[options[index].plant] += 1;
            count += 1;
        }
    }

}
