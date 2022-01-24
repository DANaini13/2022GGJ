using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    static public PlayerManager instance;
    public ParticleSystem p1_red;
    public ParticleSystem p2_blue;
    public Animator special;

    public void Awake()
    {
        instance = this;
    }

    public PlayerControl player_1;
    public PlayerControl player_2;

    public void SwapPlayer()
    {
        var temp_pos = player_1.idle_pos;
        player_1.idle_pos = player_2.idle_pos;
        player_2.idle_pos = temp_pos;
        var ps1 = Instantiate(p1_red, player_1.transform);
        ps1.Play();
        var ps2 = Instantiate(p2_blue, player_2.transform);
        ps2.Play();
        special.SetTrigger("change");
    }

    public void SetP1Hurt()
    {
        player_1.OnHurt();
        special.SetTrigger("p1hurt");
    }

    public void SetP2Hurt()
    {
        player_2.OnHurt();
        special.SetTrigger("p2hurt");
    }
}
