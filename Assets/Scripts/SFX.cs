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

    public AudioClip jackSong;
    private AudioSource jackAS;

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
            jackAS = transform.Find("Jack").GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        AS.volume = PlayerPrefs.GetFloat("SFX", 1);
        jackAS.volume = PlayerPrefs.GetFloat("SFX", 1);

        bool found = false;
        foreach (JackInTheBox a in FindObjectsByType<JackInTheBox>(FindObjectsSortMode.None))
        {
            if (!a.displayOnly && a.projectile != null)
            {
                found = true;
                if (!jackAS.isPlaying) jackAS.Play();
                break;
            }
        }
        if (!found) jackAS.Stop();
    }

    public void Play(AudioClip clip, bool singular = false)
    {
        int cur;
        cooldown.TryGetValue(clip, out cur);
        if (singular && cur > 0) return;
        AS.PlayOneShot(clip, 1 - 0.2f * cur);
        StartCoroutine(refresh(clip, singular));
    }

    public void PlayLoop(AudioClip clip)
    {
        AS.clip = clip;
        AS.Play();
    }

    private IEnumerator refresh(AudioClip clip, bool singular)
    {
        if (cooldown.ContainsKey(clip)) cooldown[clip] += 1;
        else cooldown[clip] = 1;
        yield return new WaitForSeconds(singular ? 2 : 0.25f);
        cooldown[clip] -= 1;
    }

}
