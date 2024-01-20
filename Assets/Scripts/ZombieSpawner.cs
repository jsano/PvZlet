using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{

    public float preparation;

    public GameObject[] allZombies;

    public List<List<int>> waves = new List<List<int>>();
    public int lanes;
    public static float[] ROW_TO_WORLD; // length = lanes + 1, [0] is buffer
    [HideInInspector] public int currentBuild = 0;
    private float forceSend;

    void Awake()
    {
        ROW_TO_WORLD = new float[lanes + 1];
    }

    // Start is called before the first frame update
    void Start()
    {
        waves.Add(new List<int>(new int[] { 0 }));
        waves.Add(new List<int>(new int[] { 0, 1 }));
        waves.Add(new List<int>(new int[] { 0, 0 }));
        waves.Add(new List<int>(new int[] { 0, 0 }));
        waves.Add(new List<int>(new int[] { 0, 0 }));
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
