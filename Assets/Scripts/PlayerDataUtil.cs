using System;
using UnityEngine;

public class PlayerDataUtil: MonoBehaviour
{
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
        }
    }
}