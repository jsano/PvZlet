using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour
{

    public Camera mainCamera;
    public GameObject UI;
    public GameObject ready;
    public GameObject gameOver;
    public ZombieSpawner zombieSpawner;
    public Sky sky;
    
    public GameObject reward;

    public enum Status
    {
        Start,
        Won,
        Lost
    }

    public static Status status;

    // Start is called before the first frame update
    void Start()
    {
        UI.SetActive(false);
        StartCoroutine(Start_Helper());
    }

    private IEnumerator Start_Helper()
    {
        yield return new WaitForSeconds(1);
        float acceleration = 10f;
        float midpoint = mainCamera.transform.position.x + 7.5f / 2;
        float speed = 0;
        while (mainCamera.transform.position.x < 7.5f)
        {
            if (mainCamera.transform.position.x < midpoint) speed += acceleration * Time.deltaTime;
            else speed = Mathf.Max(0.1f, speed - acceleration * Time.deltaTime);
            mainCamera.transform.Translate(Vector3.right * speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(1);
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
        zombieSpawner.enabled = true;
        sky.enabled = true;
        status = Status.Start;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        EventSystem.current.SetSelectedGameObject(null);
    }

}
