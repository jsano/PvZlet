using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{

    private AudioSource AS;

    private static SFX instance;
    public static SFX Instance { get { return instance; } }

    private HashSet<AudioClip> cooldown = new HashSet<AudioClip>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            AS = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        AS.volume = PlayerPrefs.GetFloat("SFX", 1);
    }

    public void Play(AudioClip clip)
    {
        if (cooldown.Contains(clip)) return;
        AS.PlayOneShot(clip);
        StartCoroutine(refresh(clip));
    }

    public void PlayLoop(AudioClip clip)
    {
        AS.clip = clip;
        AS.Play();
    }

    private IEnumerator refresh(AudioClip clip)
    {
        cooldown.Add(clip);
        yield return new WaitForSeconds(0.2f);
        cooldown.Remove(clip);
    }

}
