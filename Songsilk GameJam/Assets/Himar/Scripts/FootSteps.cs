using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;
    [SerializeField]
    private AudioClip[] metalClips;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Step()
    {
        if (GetComponent<Animator>().GetFloat("Speed") > 0.1f)
        {
            AudioClip clip = GetRandomClip(clips);
            audioSource.PlayOneShot(clip);
        }
    }

    private void MetalStep()
    {
        AudioClip clip = GetRandomClip(metalClips);
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip(AudioClip[] clipList)
    {
        return clipList[UnityEngine.Random.Range(0, clipList.Length)];
    }
}
