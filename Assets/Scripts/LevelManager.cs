using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public Camera mainCamera;
    public GameObject UI;
    public GameObject ready;
    public GameObject gameOver;
    public GameObject seedSelect;
    public GameObject plants;
    public GameObject sun;
    public GameObject conveyor;
    public ZombieSpawner zombieSpawner;
    public Sky sky;
    private Level l;
    private PlantBuilder pb;

    private bool letsRock;
    
    public GameObject reward;

    public enum Status
    {
        Intro,
        Start,
        Won,
        Lost
    }

    public static Status status;

    void Awake()
    {
        status = Status.Intro;
    }

    // Start is called before the first frame update
    void Start()
    {
        pb = GameObject.Find("PlantBuilder").GetComponent<PlantBuilder>();
        l = FindFirstObjectByType<Level>();
        UI.SetActive(false);
        StartCoroutine(Start_Helper());
    }

    private IEnumerator Start_Helper()
    {
        yield return new WaitForSeconds(1);
        float acceleration = 15f;
        float midpoint = mainCamera.transform.position.x + 5f;
        float speed = 0;
        while (mainCamera.transform.position.x < 10f)
        {
            if (mainCamera.transform.position.x < midpoint) speed += acceleration * Time.deltaTime;
            else speed = Mathf.Max(0.1f, speed - acceleration * Time.deltaTime);
            mainCamera.transform.Translate(Vector3.right * speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        if (l.conveyor.Count == 0)
        {
            sun.SetActive(true);
            plants.SetActive(true);
            seedSelect.SetActive(true);
            RectTransform BG = seedSelect.transform.Find("BG").GetComponent<RectTransform>();
            while (BG.transform.localPosition.y < 0)
            {
                BG.anchoredPosition = BG.anchoredPosition + Vector2.up * 2000 * Time.deltaTime;
                yield return null;
            }
            yield return new WaitUntil(() => letsRock);
            seedSelect.transform.Find("Start").gameObject.SetActive(false);
            foreach (Transform g in pb.transform.Find("Plants")) g.GetComponent<Button>().interactable = false;
            while (BG.transform.localPosition.y > -400)
            {
                BG.anchoredPosition = BG.anchoredPosition + Vector2.down * 2000 * Time.deltaTime;
                yield return null;
            }
            seedSelect.SetActive(false);
        }
        
        yield return new WaitForSeconds(0.5f);
        speed = 0;
        while (mainCamera.transform.position.x > 0)
        {
            if (mainCamera.transform.position.x > midpoint) speed += acceleration * Time.deltaTime;
            else speed = Mathf.Max(0.1f, speed - acceleration * Time.deltaTime);
            mainCamera.transform.Translate(Vector3.left * speed * Time.deltaTime);
            yield return null;
        }
        mainCamera.transform.position = new Vector3(0, 0, mainCamera.transform.position.z);
        yield return new WaitForSeconds(0.5f);
        if (l.fogColumn > 0)
        {
            sky.enabled = true;
            yield return new WaitForSeconds(1f);
        }
        ready.SetActive(true);
        TextMeshProUGUI t = ready.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        yield return new WaitForSeconds(0.5f);
        t.text = "Set...";
        yield return new WaitForSeconds(0.5f);
        t.fontSize += 16;
        t.text = "PLANT!";
        yield return new WaitForSeconds(1f);
        ready.SetActive(false);
        UI.SetActive(true);
        zombieSpawner.levelUI.SetActive(true);
        sky.enabled = true;
        status = Status.Start;
        if (l.conveyor.Count > 0)
        {
            conveyor.SetActive(true);
            RectTransform c = conveyor.GetComponent<RectTransform>();
            while (c.anchoredPosition.x < 40)
            {
                c.anchoredPosition = c.anchoredPosition + Vector2.right * 500 * Time.deltaTime;
                yield return null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LetsRock()
    {
        letsRock = true;
    }

    public void Win()
    {
        status = Status.Won;
        Instantiate(reward);
    }

    public void Lose()
    {
        status = Status.Lost;
        Time.timeScale = 1;
        gameOver.SetActive(true);
        UI.SetActive(false);
        pb.gameObject.SetActive(false);
        zombieSpawner.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

}
