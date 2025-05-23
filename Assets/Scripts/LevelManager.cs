using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public Camera mainCamera;
    public GameObject UI;
    public GameObject pause;
    public GameObject fastForward;
    public GameObject almanac;
    public GameObject shovel;
    public GameObject ready;
    public GameObject gameOver;
    public GameObject seedSelect;
    public GameObject plants;
    public GameObject sun;
    public GameObject conveyor;
    public Sky sky;
    public AudioSource music;

    public AudioClip letsRockSFX;
    public AudioClip readySetPlant;
    public AudioClip rollIn;

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
        if (Level.currentLevel == null) // TEMP
        { 
            Level.currentLevel = GameObject.Find("Testing").GetComponent<Level>();
            DontDestroyOnLoad(Level.currentLevel);
        }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        foreach (GameObject g in Level.currentLevel.special) Instantiate(g);
        foreach (Transform t in UI.transform) t.gameObject.SetActive(false);
        music.clip = music.GetComponent<Music>().allMusic[1];
        music.Play();
        StartCoroutine(Start_Helper());
    }

    protected virtual IEnumerator Start_Helper()
    {
        yield return BeforeReadySetPlant();
        yield return ReadySetPlant();
    }

    protected IEnumerator BeforeReadySetPlant()
    {
        yield return new WaitForSeconds(1);
        Vector3 startPos = mainCamera.transform.position;
        float moveTime = 2f;
        float curTime = 0;
        while (curTime < moveTime)
        {
            float point = -0.5f * Mathf.Atan(1f * Mathf.Cos(Mathf.PI / moveTime * curTime)) / Mathf.Atan(1f) + 0.5f;
            mainCamera.transform.position = startPos + new Vector3(10f * point, 0, 0);
            curTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        if (Level.currentLevel.conveyor.Count == 0)
        {
            pause.SetActive(true);
            almanac.SetActive(true);
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
            pause.SetActive(false);
            almanac.SetActive(false);
            seedSelect.transform.Find("Start").gameObject.SetActive(false);
            foreach (Transform g in PlantBuilder.Instance.transform.Find("Plants")) g.GetComponent<Button>().interactable = false;
            while (BG.transform.localPosition.y > -400)
            {
                BG.anchoredPosition = BG.anchoredPosition + Vector2.down * 2000 * Time.deltaTime;
                yield return null;
            }
            seedSelect.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);
        while (curTime > 0)
        {
            float point = -0.5f * Mathf.Atan(1f * Mathf.Cos(Mathf.PI / moveTime * curTime)) / Mathf.Atan(1f) + 0.5f;
            mainCamera.transform.position = startPos + new Vector3(10f * point, 0, 0);
            curTime -= Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = startPos;
        yield return new WaitForSeconds(0.5f);
        if (Level.currentLevel.fogColumn > 0)
        {
            sky.enabled = true;
            yield return new WaitForSeconds(1f);
        }
        music.GetComponent<Music>().FadeOut(1);
    }

    protected IEnumerator ReadySetPlant()
    {
        SFX.Instance.Play(readySetPlant);
        ready.SetActive(true);
        TextMeshProUGUI t = ready.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        yield return new WaitForSeconds(0.7f);
        t.text = "Set...";
        yield return new WaitForSeconds(0.7f);
        t.fontSize += 16;
        t.text = "PLANT!";
        yield return new WaitForSeconds(1f);
        ready.SetActive(false);
        pause.SetActive(true);
        fastForward.SetActive(true);
        shovel.SetActive(true);
        ZombieSpawner.Instance.levelUI.SetActive(true);
        sky.enabled = true;
        status = Status.Start;
        if (Level.currentLevel.conveyor.Count > 0)
        {
            SFX.Instance.Play(rollIn);
            conveyor.SetActive(true);
            RectTransform c = conveyor.GetComponent<RectTransform>();
            while (c.anchoredPosition.x < 40)
            {
                c.anchoredPosition = c.anchoredPosition + Vector2.right * 500 * Time.deltaTime;
                yield return null;
            }
        }
        if (Level.currentLevel.music != -1)
        {
            music.clip = music.GetComponent<Music>().allMusic[Level.currentLevel.music];
            music.Play();
        }
        else music.clip = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LetsRock()
    {
        SFX.Instance.Play(letsRockSFX);
        letsRock = true;
    }

    public void Win()
    {
        status = Status.Won;
        pause.SetActive(false);
        fastForward.SetActive(false);
        if (Time.timeScale > 1) UI.GetComponent<UI>().FastForward(); // unfastforward
        music.GetComponent<Music>().FadeOut(2.5f);
        Instantiate(reward);
    }

    public void Lose()
    {
        music.GetComponent<Music>().FadeOut(0.5f);
        status = Status.Lost;
        if (Time.timeScale > 1) UI.GetComponent<UI>().FastForward(); // unfastforward
        gameOver.SetActive(true);
        UI.SetActive(false);
        PlantBuilder.Instance.gameObject.SetActive(false);
        ZombieSpawner.Instance.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

}
