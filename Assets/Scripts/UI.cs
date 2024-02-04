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

}
