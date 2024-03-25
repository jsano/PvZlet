using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{

    [HideInInspector] public string levelName;

    public enum Setting
    {
        Day,
        Night,
        Pool,
        Fog,
        Roof,
        RoofNight
    }

    [Serializable]
    public class Data
    {
        public int plant;
        public int count;
    }

    public Setting setting;

    public int startingSun;

    public int unlockedUntil;
    public List<Data> conveyor = new List<Data>();

    public TextAsset waves;

    /// <summary> If >0, fog will cover this column and beyond </summary>
    public int fogColumn;

    /// <summary> If >0, all columns up to and including this column will have preplanted flower pots </summary>
    public int potColumn;

    /// <summary> The amount of time in seconds to wait before sending the first wave </summary>
    public float preparation;

    public GameObject special;

    void Start()
    {
        levelName = waves.name;
    }

    public static void LoadLevel(Level l)
    {
        Level g = Instantiate(l);
        DontDestroyOnLoad(g);
        Time.timeScale = 1;
        SceneManager.LoadScene(l.setting.ToString());
    }

}
