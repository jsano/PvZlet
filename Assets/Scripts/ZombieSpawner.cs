using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{

    private class ZombieData
    {
        public int count;
        public int ID;
        public int row;
    }

    private class GraveData
    {
        public int row;
        public int col;
    }

    private float preparation;

    /// <summary> The master list of all zombies in the game </summary>
    public GameObject[] allZombies;
    public GameObject grave;

    private List<List<ZombieData>> waves = new List<List<ZombieData>>();
    private List<List<GraveData>> graves = new List<List<GraveData>>();

    /// <summary> How many lanes are in this level. Will likely either be 5 or 6 </summary>
    public int lanes;
    /// <summary> The "amount" of zombies currently in the lawn, influenced by their <c>spawnScores</c>. When low enough, the next wave will spawn </summary>
    [HideInInspector] public int currentBuild = 0;
    /// <summary> The amount of time in seconds to wait before the next wave spawns no matter what </summary>
    private float forceSend;
    private int waveNumber;

    // Start is called before the first frame update
    void Start()
    {
        Level l = FindFirstObjectByType<Level>();
        preparation = l.preparation;
        TextAsset levelZombies = l.waves;
        string[] level = levelZombies.text.Split(new string[] { " ", "\n" }, StringSplitOptions.None);
        List<ZombieData> wave = new List<ZombieData>();
        List<GraveData> grave = new List<GraveData>();
        for (int i = 0; i < level.Length;)
        {
            if (level[i].Contains("-"))
            {
                waves.Add(wave);
                graves.Add(grave);
                wave = new List<ZombieData>();
                grave = new List<GraveData>();
                i += 1;
                continue;
            }
            try
            {
                wave.Add(new ZombieData { count = int.Parse(level[i]), ID = int.Parse(level[i + 1]), row = int.Parse(level[i + 2]) });
                i += 3;
            }
            catch (FormatException)
            {
                grave.Add(new GraveData { row = int.Parse(level[i+1]), col = int.Parse(level[i+2]) });
                i += 3;
            }
        }
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        preparation = Mathf.Max(0, preparation - Time.deltaTime);
        forceSend -= Time.deltaTime;
    }

    private IEnumerator Spawn()
    {
        yield return new WaitUntil(() => preparation <= 0);
        GameObject.Find("UI").GetComponent<UI>().ShowProgress();
        for (waveNumber = 0; waveNumber < waves.Count; waveNumber++)
        {
            if (Player.lost) yield break;
            forceSend = 30f;

            foreach (GraveData c in graves[waveNumber])
            {
                Tile.tileObjects[c.row, c.col].Place(grave);
            }

            foreach (ZombieData i in waves[waveNumber])
            {
                for (int x = 0; x < i.count; x++)
                {
                    if (i.ID == 15) // Bobsled
                    {
                        List<int> possible = new List<int>();
                        for (int l = 1; l <= lanes; l++) {
                            Tile check = Tile.tileObjects[l, 9];
                            if (check.gridItem != null && check.gridItem.tag == "Snow") possible.Add(l);
                        }
                        if (possible.Count > 0)
                        {
                            currentBuild += allZombies[i.ID].GetComponent<Zombie>().spawnScore;
                            GameObject g1 = Instantiate(allZombies[i.ID]);
                            g1.GetComponent<Zombie>().row = possible[UnityEngine.Random.Range(0, possible.Count)];
                        }
                        continue;
                    }
                    currentBuild += allZombies[i.ID].GetComponent<Zombie>().spawnScore;
                    GameObject g = Instantiate(allZombies[i.ID]);
                    int lane = i.row;
                    if (lane == 0)
                    {
                        if (!g.GetComponent<Zombie>().aquatic)
                            do lane = UnityEngine.Random.Range(1, lanes + 1); while (lane == 3 || lane == 4);
                        else lane = UnityEngine.Random.Range(3, 5);
                        if (g.GetComponent<Balloon>() != null) lane = UnityEngine.Random.Range(1, lanes+1);
                    }
                    g.GetComponent<Zombie>().row = lane;
                    yield return new WaitForSeconds(0.2f);
                }
            }
            float maxBuild = currentBuild;
            yield return new WaitUntil(() => (currentBuild / maxBuild < 0.5f) || forceSend <= 0);
        }
        yield return new WaitUntil(() => currentBuild == 0);
        FindFirstObjectByType<Level>().Win();
    }

    public float CompletedPercentage()
    {
        if (waves.Count == 1) return 1;
        return ((float) waveNumber) / (waves.Count - 1);
    }

}
