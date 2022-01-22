using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    static public PlayerManager instance;

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
    }
}
