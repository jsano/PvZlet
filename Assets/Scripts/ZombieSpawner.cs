using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{

    /// <summary> The amount of time in seconds to wait before sending the first wave </summary>
    public float preparation;

    /// <summary> The master list of all zombies in the game </summary>
    public GameObject[] allZombies;

    public List<List<int>> waves = new List<List<int>>();
    /// <summary> How many lanes are in this level. Will likely either be 5 or 6 </summary>
    public int lanes;
    /// <summary> The global mapping between row number to world y-coordinates. Length is <c>lanes+1</c> with index 0 as a buffer </summary>
    public static float[] ROW_TO_WORLD;
    /// <summary> The "amount" of zombies currently in the lawn, influenced by their <c>spawnScores</c>. When low enough, the next wave will spawn </summary>
    [HideInInspector] public int currentBuild = 0;
    /// <summary> The amount of time in seconds to wait before the next wave spawns no matter what </summary>
    private float forceSend;

    void Awake()
    {
        ROW_TO_WORLD = new float[lanes + 1];
    }

    // Start is called before the first frame update
    void Start()
    {
        waves.Add(new List<int>(new int[] { allZombies.Length - 1 }));
        waves.Add(new List<int>(new int[] { 0, 1 }));
        waves.Add(new List<int>(new int[] { 2, 3 }));
        waves.Add(new List<int>(new int[] { 4, 5 }));
        waves.Add(new List<int>(new int[] { 6, 7 }));
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
        for (int waveNumber = 0; waveNumber < waves.Count; waveNumber++)
        {
            forceSend = 30f;
            foreach (int i in waves[waveNumber])
            {
                currentBuild += allZombies[i].GetComponent<Zombie>().spawnScore;
                GameObject g = Instantiate(allZombies[i]);
                g.GetComponent<Zombie>().row = Random.Range(1, lanes);
            }
            int maxBuild = currentBuild;
            yield return new WaitUntil(() => currentBuild / maxBuild < 0.5f || forceSend <= 0);
        }
        yield return new WaitUntil(() => currentBuild == 0);
        Debug.Log("WIN");
    }

}
