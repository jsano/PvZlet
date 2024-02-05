using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour
{

    public Camera mainCamera;
    public GameObject UI;
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
        while (mainCamera.transform.position.x < 5)
        {
            mainCamera.transform.Translate(Vector3.right * 5 * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        while (mainCamera.transform.position.x > 0)
        {
            mainCamera.transform.Translate(Vector3.left * 5 * Time.deltaTime);
            yield return null;
        }
        mainCamera.transform.position = new Vector3(0, 0, mainCamera.transform.position.z);
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
        gameOver.SetActive(true);
        UI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

}
