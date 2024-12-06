using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;
    [SerializeField] private AudioClip bgmRepeat;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bgm != null)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = bgmRepeat;
                audioSource.Play();
            }
        }
    }
}
