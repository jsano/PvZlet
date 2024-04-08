using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieSpawnerZomboss : ZombieSpawner
{

    private Zomboss z;

    public override void Awake()
    {
        
    }

    // Update is called once per frame
    public override void Start()
    {
        Instance.levelUI.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = Level.currentLevel.waves.name;
        Instance.levelUI.transform.Find("Progress").gameObject.SetActive(true);
    }

    public override void Update()
    {
        if (LevelManager.status == LevelManager.Status.Start)
        {
            if (z == null) z = FindFirstObjectByType<Zomboss>();
            Instance.progressBar.fillAmount = 1 - z.HP / z.getBaseHP();
            if (z.HP <= 0)
            {
                Instance.levelManager.Win();
            }
        }
    }

}
