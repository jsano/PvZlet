using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerZomboss : LevelManager
{

    void Awake()
    {
        LevelManager[] l = FindObjectsByType<LevelManager>(FindObjectsSortMode.None);
        foreach (LevelManager l1 in l) 
        {
            if (l1 != this) Destroy(l1.gameObject);
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        foreach (Transform t in UI.transform) t.gameObject.SetActive(false);
        StartCoroutine(Start_Helper());
    }

    protected override IEnumerator Start_Helper()
    {
        yield return BeforeReadySetPlant();
        Zomboss g = Instantiate(ZombieSpawner.Instance.allZombies[31]).GetComponent<Zomboss>();
        g.row = 3;
        yield return new WaitForSeconds(3.5f);
        yield return ReadySetPlant();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
