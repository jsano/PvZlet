using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private HashSet<int> flagWaveNumbers = new HashSet<int>();

    /// <summary> How many lanes are in this level. Will likely either be 5 or 6 </summary>
    public int lanes;
    /// <summary> The "amount" of zombies currently in the lawn, influenced by their <c>spawnScores</c>. When low enough, the next wave will spawn </summary>
    private int currentBuild = 0;
    /// <summary> The amount of time in seconds to wait before the next wave spawns no matter what </summary>
    private float forceSend;
    private int waveNumber;

    public LevelManager levelManager;
    public Image progressBar;
    public GameObject hugeWave;
    public GameObject final;
    public GameObject levelUI;
    public GameObject flag;

    private List<GameObject> displayZombies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Level l = FindFirstObjectByType<Level>();
        levelUI.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = l.levelName;
        HashSet<int> unique = new HashSet<int>();
        lanes = (l.setting == Level.Setting.Pool || l.setting == Level.Setting.Fog) ? 6 : 5;
        preparation = l.preparation;
        TextAsset levelZombies = l.waves;
        string[] level = levelZombies.text.Split(new string[] { " ", "\n" }, StringSplitOptions.None);
        List<ZombieData> wave = new List<ZombieData>();
        List<GraveData> grave = new List<GraveData>();
        for (int i = 0; i < level.Length;)
        {
            if (level[i].Contains("-"))
            {
                wave = wave.OrderBy(_ => UnityEngine.Random.Range(0, 1f)).ToList();
                waves.Add(wave);
                graves.Add(grave);
                wave = new List<ZombieData>();
                grave = new List<GraveData>();
                i += 1;
                continue;
            }
            try
            {
                int _ID = int.Parse(level[i + 1]);
                wave.Add(new ZombieData { count = int.Parse(level[i]), ID = _ID, row = int.Parse(level[i + 2]) });
                if (_ID == 1) flagWaveNumbers.Add(waves.Count);
                else unique.Add(_ID);
                i += 3;
            }
            catch (FormatException)
            {
                grave.Add(new GraveData { row = int.Parse(level[i+1]), col = int.Parse(level[i+2]) });
                i += 3;
            }
        }

        foreach (int i in flagWaveNumbers)
        {
            Transform p = levelUI.transform.Find("Progress");
            GameObject g = Instantiate(flag, p);
            g.GetComponent<RectTransform>().anchoredPosition = new Vector3(-(float)i / (waves.Count - 1) * (p.GetComponent<RectTransform>().rect.width - 10) - 5, 0, 0);
        }
        transform.Find("Display").position = new Vector3(17.5f, 0, 0);
        foreach (int i in unique)
        {
            Vector3 offset = new Vector2(UnityEngine.Random.Range(-2, 2f), UnityEngine.Random.Range(-5, 5f));
            allZombies[i].SetActive(false);
            GameObject g = Instantiate(allZombies[i], transform.Find("Display").transform.position + offset, Quaternion.identity);
            g.GetComponent<Zombie>().displayOnly = true;
            displayZombies.Add(g);
            g.SetActive(true);
            allZombies[i].SetActive(true);
        }
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.status == LevelManager.Status.Start)
        {
            preparation = Mathf.Max(0, preparation - Time.deltaTime);
            forceSend -= Time.deltaTime;
        }
    }

    private IEnumerator Spawn()
    {
        yield return new WaitUntil(() => preparation <= 0 && LevelManager.status == LevelManager.Status.Start);
        foreach (GameObject g in displayZombies) Destroy(g);
        levelUI.transform.Find("Progress").gameObject.SetActive(true);
        for (waveNumber = 0; waveNumber < waves.Count; waveNumber++)
        {
            if (waveNumber == waves.Count - 1) StartCoroutine(Final());
            
            progressBar.fillAmount = (waves.Count == 1) ? 1 : ((float)waveNumber) / (waves.Count - 1);
            currentBuild = 0;
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
                            if (i.row != 0) g1.GetComponent<Zombie>().row = i.row;
                            g1.GetComponent<Zombie>().waveNumber = waveNumber;
                            yield return new WaitForSeconds(0.2f);
                        }
                        continue;
                    }
                    currentBuild += allZombies[i.ID].GetComponent<Zombie>().spawnScore;
                    GameObject g = Instantiate(allZombies[i.ID]);
                    int lane = i.row;
                    if (lane == 0)
                    {
                        if (!g.GetComponent<Zombie>().aquatic)
                        {
                            if (lanes == 6) do lane = UnityEngine.Random.Range(1, lanes + 1); while (lane == 3 || lane == 4);
                            else lane = UnityEngine.Random.Range(1, lanes + 1);
                        }
                        else lane = UnityEngine.Random.Range(3, 5);
                        if (g.GetComponent<Balloon>() != null) lane = UnityEngine.Random.Range(1, lanes+1);
                    }
                    g.GetComponent<Zombie>().row = lane;
                    g.GetComponent<Zombie>().waveNumber = waveNumber;
                    yield return new WaitForSeconds(0.2f);
                }
            }

            if (flagWaveNumbers.Contains(waveNumber))
            {
                for (int i = 1; i <= lanes; i++)
                    for (int j = 1; j <= 9; j++)
                    {
                        Tile t = Tile.tileObjects[i, j];
                        if (t.gridItem != null && t.gridItem.tag == "Grave")
                        {
                            int[] possible = new int[] {0, 2, 4};
                            int _ID = possible[UnityEngine.Random.Range(0, possible.Length)];
                            currentBuild += allZombies[_ID].GetComponent<Zombie>().spawnScore;
                            GameObject g = Instantiate(allZombies[_ID], t.transform.position, Quaternion.identity);
                            g.GetComponent<Zombie>().row = i;
                            g.GetComponent<Zombie>().waveNumber = waveNumber;
                        }
                    }    
            }

            float maxBuild = currentBuild;

            if (flagWaveNumbers.Contains(waveNumber + 1))
            {
                yield return new WaitUntil(() => currentBuild == 0 || forceSend <= 0);
                yield return new WaitForSeconds(2);
                hugeWave.SetActive(true);
                TextMeshProUGUI t = hugeWave.GetComponent<TextMeshProUGUI>();
                while (t.color.a < 1)
                {
                    t.color += new Color(0, 0, 0, Time.deltaTime * 10);
                    yield return null;
                }
                yield return new WaitForSeconds(4);
                while (t.color.a > 0)
                {
                    t.color -= new Color(0, 0, 0, Time.deltaTime * 10);
                    yield return null;
                }
                hugeWave.SetActive(false);
                yield return new WaitForSeconds(1);
            }
            else yield return new WaitUntil(() => (currentBuild / maxBuild < 0.5f) || forceSend <= 0);
        }
        yield return new WaitUntil(() => currentBuild <= 0);
        levelManager.Win();
    }

    private IEnumerator Final()
    {
        final.SetActive(true);
        TextMeshProUGUI t = final.GetComponent<TextMeshProUGUI>();
        while (t.color.a < 1)
        {
            t.color += new Color(0, 0, 0, Time.deltaTime * 10);
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        while (t.color.a > 0)
        {
            t.color -= new Color(0, 0, 0, Time.deltaTime * 10);
            yield return null;
        }
        final.SetActive(false);
    }

    public void SubtractBuild(int build, int wave)
    {
        if (waveNumber == wave || waveNumber >= waves.Count) currentBuild -= build;
    }

}
