using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlaySnd : MonoBehaviour
{
    public AudioClip[] clips;
    void Start()
    {
        AudioSource audio = this.gameObject.GetComponent<AudioSource>();
        // audio.pitch = Random.Range(0.5f, 1.5f);
        audio.clip = clips[Random.Range(0, clips.Length)];
        audio.Play();
    }
}
