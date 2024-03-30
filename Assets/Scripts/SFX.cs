using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{

    private AudioSource AS;

    private static SFX instance;
    public static SFX Instance { get { return instance; } }

    private Dictionary<AudioClip, int> cooldown = new Dictionary<AudioClip, int>();

    public AudioClip[] zombieEat;
    public AudioClip zombieEatNut; // Idk how else to store sounds globally like this
    public AudioClip gulp;
    public AudioClip hypnotize;

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
        int cur;
        cooldown.TryGetValue(clip, out cur);
        AS.PlayOneShot(clip, 1 - 0.2f * cur);
        StartCoroutine(refresh(clip));
    }

    public void PlayLoop(AudioClip clip)
    {
        AS.clip = clip;
        AS.Play();
    }

    private IEnumerator refresh(AudioClip clip)
    {
        if (cooldown.ContainsKey(clip)) cooldown[clip] += 1;
        else cooldown[clip] = 1;
        yield return new WaitForSeconds(0.25f);
        cooldown[clip] -= 1;
    }

}
