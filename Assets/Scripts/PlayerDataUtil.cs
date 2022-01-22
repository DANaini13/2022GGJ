using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerDataUtil: MonoBehaviour
{
    public GameObject game_over_img;
    public Slider p1_slider;
    public Slider p2_slider;
    
    static public PlayerDataUtil Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (game_over)
        {
            game_over = false;
            Time.timeScale = 0;
            game_over_img.SetActive(true);
            DOVirtual.DelayedCall(3, () =>
            {
                SceneManager.LoadScene(0);
            }).SetUpdate(true);
        }
    }

    private int p1_health = 100;
    private int p2_health = 100;
    private bool game_over = false;

    public int P1Health
    {
        get
        {
            return p1_health;
        }
        set
        {
            p1_health = value;
            if (p1_health > 100) p1_health = 100;
            if (p1_health <= 0)
            {
                game_over = true;
                p1_health = 0;
            }

            p1_slider.value = p1_health / 100.0f;
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
            if (p2_health > 100) p2_health = 100;
            if (p2_health <= 0)
            {
                game_over = true;
                p2_health = 0;
            }
            p2_slider.value = p2_health / 100.0f;
        }
    }
}