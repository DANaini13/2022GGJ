using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerDataUtil: MonoBehaviour
{
    public Text player_1_text;
    public Text player_2_text;
    
    static public PlayerDataUtil Instance;

    private void Awake()
    {
        Instance = this;
    }

    private int p1_health = 100;
    private int p2_health = 100;

    public int P1Health
    {
        get
        {
            return p1_health;
        }
        set
        {
            p1_health = value;
            player_1_text.text = p1_health.ToString();
        }
    }
    
    public int P2Health
    {
        get
        {
            return p2_health;
        }
        set
        {
            p2_health = value;
            player_2_text.text = p2_health.ToString();
        }
    }
}