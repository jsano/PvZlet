using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZombieSpawner : MonoBehaviour
{

    private static ZombieSpawner instance;
    public static ZombieSpawner Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            l = FindFirstObjectByType<Level>();
            if (l == null) return;
            lanes = (l.setting == Level.Setting.Pool || l.setting == Level.Setting.Fog) ? 6 : 5;
        }
    }

    private class ZombieData
    {
        public int count;
        public int ID;
        public int row;
        public int col;
    }
    /* TODO: MAYBE HANDLE GROUPS
    group
    2 0
    2 2
    2 4
    endgroup  <-all together in one random lane
     */

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
    private int[] waveBuilds;
    /// <summary> The amount of time in seconds to wait before the next wave spawns no matter what </summary>
    private float forceSend;
    private int waveNumber;

    private Level l;
    public LevelManager levelManager;
    public Image progressBar;
    public GameObject hugeWave;
    public GameObject final;
    public GameObject levelUI;
    public GameObject flag;

    private List<GameObject> displayZombies = new List<GameObject>();

    public AudioClip graveRiseSFX;
    public AudioClip zombiesAreComingSFX;
    public AudioClip hugeWaveSFX;
    public AudioClip hugeWaveStartSFX;
    public AudioClip finalWaveSFX;

    // Start is called before the first frame update
    void Start()
    {
        levelUI.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = l.levelName;
        HashSet<int> unique = new HashSet<int>();
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
                if (_ID == 22)
                {
                    wave.Add(new ZombieData { count = int.Parse(level[i]), ID = _ID, row = int.Parse(level[i + 2]), col = int.Parse(level[i + 3]) });
                    i += 4;
                }
                else
                {
                    wave.Add(new ZombieData { count = int.Parse(level[i]), ID = _ID, row = int.Parse(level[i + 2]) });
                    i += 3;
                }
                if (_ID == 1) flagWaveNumbers.Add(waves.Count);
                else unique.Add(_ID);
            }
            catch (FormatException)
            {
                grave.Add(new GraveData { row = int.Parse(level[i + 1]), col = int.Parse(level[i + 2]) });
                i += 3;
            }
        }
        waveBuilds = new int[waves.Count];

        foreach (int i in flagWaveNumbers)
        {
            Transform p = levelUI.transform.Find("Progress");
            GameObject g = Instantiate(flag, p);
            g.GetComponent<RectTransform>().anchoredPosition = new Vector3(-(float)i / (waves.Count - 1) * (p.GetComponent<RectTransform>().rect.width - 10) - 5, 0, 0);
        }
        transform.Find("Display").position = new Vector3(17.5f, 0, 0);
        int sortingOrder = 0;
        foreach (int i in unique)
        {
            Vector3 offset = new Vector2(UnityEngine.Random.Range(-2, 2f), UnityEngine.Random.Range(-5, 5f));
            allZombies[i].SetActive(false);
            GameObject g = Instantiate(allZombies[i], transform.Find("Display").transform.position + offset, Quaternion.identity);
            g.GetComponent<Zombie>().displayOnly = true;
            g.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            g.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
            displayZombies.Add(g);
            g.SetActive(true);
            allZombies[i].SetActive(true);
            g.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
            sortingOrder += 3;
        }
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.status == LevelManager.Status.Intro)
            foreach (GameObject g in displayZombies) g.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (LevelManager.status == LevelManager.Status.Start)
        {
            preparation = Mathf.Max(0, preparation - Time.deltaTime);
            forceSend -= Time.deltaTime;
        }
    }

    private IEnumerator Spawn()
    {
        yield return new WaitUntil(() => preparation <= 0 && LevelManager.status == LevelManager.Status.Start);
        SFX.Instance.Play(zombiesAreComingSFX);
        foreach (GameObject g in displayZombies) Destroy(g);
        levelUI.transform.Find("Progress").gameObject.SetActive(true);
        for (waveNumber = 0; waveNumber < waves.Count; waveNumber++)
        {
            if (waveNumber == waves.Count - 1) StartCoroutine(Final());

            progressBar.fillAmount = (waves.Count == 1) ? 1 : ((float)waveNumber) / (waves.Count - 1);
            forceSend = 30f;

            foreach (GraveData c in graves[waveNumber])
            {
                Tile.tileObjects[c.row, c.col].Place(grave);
            }

            int sortingOrder = 0;
            foreach (ZombieData i in waves[waveNumber])
            {
                List<int> possible = new List<int>();
                if (i.row != 0) possible.Add(i.row);
                else
                {
                    if (i.ID == 15) // Bobsled
                    {
                        for (int l = 1; l <= lanes; l++)
                        {
                            if (Tile.tileObjects[l, 9].ContainsGridItem("Snow")) possible.Add(l);
                        }
                        if (possible.Count == 0) continue;
                    }
                    else if (i.ID == 18) possible.AddRange(Enumerable.Range(1, lanes)); // Balloon
                    else if (allZombies[i.ID].GetComponent<Zombie>().aquatic) possible.AddRange(new int[] { 3, 4 });
                    else
                    {
                        if (lanes == 6) possible.AddRange(new int[] { 1, 2, 5, 6 });
                        else possible.AddRange(Enumerable.Range(1, lanes));
                    }
                }
                List<int> remaining = new List<int>(possible);
                for (int x = 0; x < i.count; x++)
                {
                    waveBuilds[waveNumber] += allZombies[i.ID].GetComponent<Zombie>().spawnScore;
                    GameObject g = Instantiate(allZombies[i.ID]);
                    if (i.ID == 22) // Bungee
                    {
                        g.GetComponent<Bungee>().row = i.row;
                        g.GetComponent<Bungee>().col = i.col;
                    }
                    else
                    {
                        int index = UnityEngine.Random.Range(0, remaining.Count);
                        g.GetComponent<Zombie>().row = remaining[index];
                        remaining.RemoveAt(index);
                        if (remaining.Count == 0) remaining = new List<int>(possible);
                    }
                    g.GetComponent<Zombie>().waveNumber = waveNumber;
                    g.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
                    sortingOrder += 3;
                    yield return new WaitForSeconds(0.2f);
                }
            }

            if (flagWaveNumbers.Contains(waveNumber))
            {
                for (int i = 1; i <= lanes; i++)
                    for (int j = 1; j <= 9; j++)
                    {
                        Tile t = Tile.tileObjects[i, j];
                        if (t.ContainsGridItem("Grave"))
                        {
                            SFX.Instance.Play(graveRiseSFX);
                            int[] possible = new int[] { 0, 2, 4 };
                            int _ID = possible[UnityEngine.Random.Range(0, possible.Length)];
                            waveBuilds[waveNumber] += allZombies[_ID].GetComponent<Zombie>().spawnScore;
                            GameObject g = Instantiate(allZombies[_ID], t.transform.position, Quaternion.identity);
                            g.GetComponent<Zombie>().row = i;
                            g.GetComponent<Zombie>().waveNumber = waveNumber;
                            g.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
                            sortingOrder += 3;
                        }
                    }
            }

            float maxBuild = waveBuilds[waveNumber];

            if (flagWaveNumbers.Contains(waveNumber + 1))
            {
                yield return new WaitUntil(() => waveBuilds[waveNumber] == 0 || forceSend <= 0);
                yield return new WaitForSeconds(2);
                SFX.Instance.Play(hugeWaveSFX);
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
                SFX.Instance.Play(hugeWaveStartSFX);
            }
            else yield return new WaitUntil(() => (waveBuilds[waveNumber] / maxBuild < 0.5f) || forceSend <= 0);
        }
        yield return new WaitUntil(() => Mathf.Max(waveBuilds) == 0);
        levelManager.Win();
    }

    private IEnumerator Final()
    {
        SFX.Instance.Play(finalWaveSFX);
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
        waveBuilds[wave] -= build;
    }

}
