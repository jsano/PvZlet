using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    private TextMeshProUGUI s;
    public GameObject pauseMenu;
    public GameObject pause;
    public GameObject fastForward;
    private float curTimeScale = 1;

    // Start is called before the first frame update
    void Start()
    {
        s = transform.Find("Sun").Find("Text").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        s.text = PlantBuilder.sun + "";
    }

    public void Pause()
    {
        if (Time.timeScale == 0) // unpause
        {
            Time.timeScale = curTimeScale;
            pauseMenu.SetActive(false);
            pause.GetComponent<Image>().color = pause.GetComponent<Button>().colors.normalColor;
        }
        else
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            pause.GetComponent<Image>().color = pause.GetComponent<Button>().colors.selectedColor;
        }
    }

    public void FastForward()
    {
        if (Time.timeScale == 2) // revert
        {
            Time.timeScale = 1;
            curTimeScale = 1;
            fastForward.GetComponent<Image>().color = fastForward.GetComponent<Button>().colors.normalColor;
        }
        else
        {
            Time.timeScale = 2;
            curTimeScale = 2;
            fastForward.GetComponent<Image>().color = fastForward.GetComponent<Button>().colors.selectedColor;
        }
    }

    public void MainMenu()
    {
        Level l = FindFirstObjectByType<Level>();
        if (l != null) SceneManager.MoveGameObjectToScene(l.gameObject, SceneManager.GetActiveScene());
        SceneManager.LoadScene(0);
    }

}
