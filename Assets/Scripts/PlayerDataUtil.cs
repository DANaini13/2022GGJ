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
    public int score = 0;
    public int counter = 0;
    public float score_add_duration = 1f;
    public Text score_text;
    
    static public PlayerDataUtil Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        CounterCheck();
        FeverTimeCheck();
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

    private void FeverTimeCheck()
    {
        if (counter < 100)
        {
            FeverTimeManager.instance.ExitFeverTime();
        }
        else
        {
            FeverTimeManager.instance.EnterFeverTime();
        }
    }

    private bool hitted = false;
    private float last_score_check_time = 0;
    private void CounterCheck()
    {
        if (Time.fixedTime - last_score_check_time < score_add_duration) return;
        last_score_check_time = Time.fixedTime;
        if (!hitted)
        {
            ++counter;
            score += counter/10 + 1;
        }
        else
        {
            counter = 0;
        }

        hitted = false;
        score_text.text = score.ToString();
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
            if (value < p1_health)
            {
                hitted = true;
            }
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
            if (value < p2_health)
            {
                hitted = true;
            }
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