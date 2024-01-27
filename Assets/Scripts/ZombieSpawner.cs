using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{

    private class Coordinates
    {
        public int row;
        public int col;
    }

    /// <summary> The amount of time in seconds to wait before sending the first wave </summary>
    public float preparation;

    /// <summary> The master list of all zombies in the game </summary>
    public GameObject[] allZombies;
    public GameObject grave;

    private List<List<int>> waves = new List<List<int>>();
    private List<List<Coordinates>> graves = new List<List<Coordinates>>();

    /// <summary> How many lanes are in this level. Will likely either be 5 or 6 </summary>
    public int lanes;
    /// <summary> The "amount" of zombies currently in the lawn, influenced by their <c>spawnScores</c>. When low enough, the next wave will spawn </summary>
    [HideInInspector] public int currentBuild = 0;
    /// <summary> The amount of time in seconds to wait before the next wave spawns no matter what </summary>
    private float forceSend;

    // Start is called before the first frame update
    void Start()
    {
        waves.Add(new List<int>(new int[] { allZombies.Length - 1, 17 })); graves.Add(new List<Coordinates>(new Coordinates[] { new Coordinates { row = 1, col = 9 } }));
        waves.Add(new List<int>(new int[] { 15, 10, 11, 12 })); graves.Add(new List<Coordinates>(new Coordinates[] { new Coordinates { row = 2, col = 9 } }));
        waves.Add(new List<int>(new int[] { 2, 3 })); graves.Add(new List<Coordinates>(new Coordinates[] { new Coordinates { row = 3, col = 9 } }));
        waves.Add(new List<int>(new int[] { 4, 5 })); graves.Add(new List<Coordinates>(new Coordinates[] { new Coordinates { row = 4, col = 9 } }));
        waves.Add(new List<int>(new int[] { 6, 7 })); graves.Add(new List<Coordinates>(new Coordinates[] { new Coordinates { row = 5, col = 9 } }));
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

            foreach (Coordinates c in graves[waveNumber])
            {
                Tile.tileObjects[c.row, c.col].Place(grave);
            }

            foreach (int i in waves[waveNumber])
            {
                if (i == 15) // Bobsled
                {
                    List<int> possible = new List<int>();
                    for (int l = 1; l <= lanes; l++) {
                        Tile check = Tile.tileObjects[l, 9];
                        if (check.gridItem != null && check.gridItem.tag == "Snow") possible.Add(l);
                    }
                    if (possible.Count > 0)
                    {
                        currentBuild += allZombies[i].GetComponent<Zombie>().spawnScore;
                        GameObject g1 = Instantiate(allZombies[i]);
                        g1.GetComponent<Zombie>().row = possible[Random.Range(0, possible.Count)];
                    }
                    continue;
                }
                currentBuild += allZombies[i].GetComponent<Zombie>().spawnScore;
                GameObject g = Instantiate(allZombies[i]);
                int lane = 1;
                if (!g.GetComponent<Zombie>().aquatic)
                    while (lane == 3 || lane == 4) lane = Random.Range(1, lanes+1);
                else lane = Random.Range(3, 5);
                if (g.GetComponent<Balloon>() != null) lane = Random.Range(1, lanes+1);
                g.GetComponent<Zombie>().row = lane;
                yield return new WaitForSeconds(0.2f);
            }
            float maxBuild = currentBuild;
            yield return new WaitUntil(() => (currentBuild / maxBuild < 0.5f) || forceSend <= 0);
        }
        yield return new WaitUntil(() => currentBuild == 0);
        Debug.Log("WIN");
    }

}
