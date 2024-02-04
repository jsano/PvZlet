using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour
{

    private TextMeshProUGUI s;
    private Transform level;
    public Image progressBar;
    public GameObject pauseMenu;
    public GameObject pause;
    public GameObject fastForward;
    private float curTimeScale = 1;

    private ZombieSpawner zs;

    // Start is called before the first frame update
    public virtual void Start()
    {
        s = transform.Find("Sun").Find("Text").GetComponent<TextMeshProUGUI>();
        level = transform.Find("Level");
        level.Find("Name").GetComponent<TextMeshProUGUI>().text = FindFirstObjectByType<Level>().levelName;
        zs = GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        s.text = PlantBuilder.sun + "";
        progressBar.fillAmount = zs.CompletedPercentage();

        if (!pauseMenu.activeInHierarchy) Time.timeScale = curTimeScale;

        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            if (raycastResults.Count == 0) EventSystem.current.SetSelectedGameObject(null);
        }
        if (Input.GetMouseButtonDown(1)) EventSystem.current.SetSelectedGameObject(null);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EventSystem.current.currentSelectedGameObject != null) EventSystem.current.SetSelectedGameObject(null);
            else Pause();
        }
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
        if (curTimeScale == 2) // revert
        {
            curTimeScale = 1;
            fastForward.GetComponent<Image>().color = fastForward.GetComponent<Button>().colors.normalColor;
        }
        else
        {
            curTimeScale = 2;
            fastForward.GetComponent<Image>().color = fastForward.GetComponent<Button>().colors.selectedColor;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Level l = FindFirstObjectByType<Level>();
        if (l != null) SceneManager.MoveGameObjectToScene(l.gameObject, SceneManager.GetActiveScene());
        SceneManager.LoadScene(0);
    }

    public void ShowProgress()
    {
        level.Find("Progress").gameObject.SetActive(true);
    }

}
