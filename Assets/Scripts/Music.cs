using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    private AudioSource AS;
    private bool changing;

    public AudioClip[] allMusic;

    // Start is called before the first frame update
    void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!changing)
        {
            float vol = PlayerPrefs.GetFloat("Music", -1);
            AS.volume = vol == -1 ? 1 : vol;
        }
    }

    public void FadeOut(float duration)
    {
        changing = true;
        StartCoroutine(FadeOut_Helper(duration));
    }

    public IEnumerator FadeOut_Helper(float duration)
    {
        float currentTime = 0;
        float start = AS.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            AS.volume = Mathf.Lerp(start, 0, currentTime / duration);
            yield return null;
        }
        AS.Pause();
        changing = false;
    }

    public void FadeIn(float duration)
    {
        changing = true;
        StartCoroutine(FadeIn_Helper(duration));
    }

    public IEnumerator FadeIn_Helper(float duration)
    {
        AS.UnPause();
        float currentTime = 0;
        float to = PlayerPrefs.GetFloat("Music", -1);
        to = to == -1 ? 1 : to;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            AS.volume = Mathf.Lerp(0, to, currentTime / duration);
            yield return null;
        }
        changing = false;
    }

}
