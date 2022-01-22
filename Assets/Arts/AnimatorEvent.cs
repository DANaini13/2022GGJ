using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvent : MonoBehaviour
{
    public ParticleSystem[] ps;
    public delegate void AnimationEventCallBack(string clip_name);
    public AnimationEventCallBack on_clip_finished = null;
    public AnimationEventCallBack on_clip_start = null;

    public void PlayPS(int index){
        ps[index].Play();
    } 
    
    private void OnClipPlayFinished(string clip_name)
    {
        if (on_clip_finished == null) return;
        on_clip_finished(clip_name);
    }

    private void OnClipStartPlay(string clip_name)
    {
        if (on_clip_start == null) return;
        on_clip_start(clip_name);
    }
}
