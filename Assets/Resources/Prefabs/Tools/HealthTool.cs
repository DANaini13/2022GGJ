using System;
using UnityEngine;


public class SceneTool : MonoBehaviour
{
    public AudioClip audio_clip;
    private AudioSource _as;

    private void Awake()
    {
        _as = GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        //_as.PlayOneShot(audio_clip);
    }
}

public class HealthTool : SceneTool
{
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        PlayAudio();
        var pc = other.gameObject.GetComponent<PlayerControl>();
        if (pc.player_id == PlayerControl.PlayerIdType.P1)
            PlayerDataUtil.Instance.P1Health += 5;
        else
            PlayerDataUtil.Instance.P2Health += 5;
        GameObject.Destroy(gameObject);
    } 
}
