using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{

    public enum Setting
    {
        Day,
        Night,
        Pool,
        Fog,
        Roof,
        RoofNight
    }

    public Setting setting;

    public int startingSun;

    public TextAsset waves;

    /// <summary> The amount of time in seconds to wait before sending the first wave </summary>
    public float preparation;

    public static void LoadLevel(Level l)
    {
        Level g = Instantiate(l);
        DontDestroyOnLoad(g);
        SceneManager.LoadScene(l.setting.ToString());
    }

}
