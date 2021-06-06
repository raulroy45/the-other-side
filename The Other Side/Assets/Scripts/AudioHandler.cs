using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    private AudioSource audioSrc;
    private float musicVolume;

    // Start is called before the first frame update
    void Start()
    {
        musicVolume = 1;
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audioSrc.volume = musicVolume;
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void SetVolume(float vol)
    {
        musicVolume = vol;
    }
}
