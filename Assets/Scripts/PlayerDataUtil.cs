using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerDataUtil : MonoBehaviour
{
    public GameObject game_over_img;
    public Slider p1_slider;
    public Slider p2_slider;
    public int score = 0;
    public int counter = 0;

    public int counter_p1;
    public int counter_p2;
    public float score_add_duration = 1f;
    public Text score_text;
    public Text gameover_score_text;
    public Text combo_text_p1;
    public Text combo_text_p2;
    public bool health_still = false;
    public Text best_combo_text;
    public Text survive_text;
    public Text achievement_text;
    private int best_combo;

    static public PlayerDataUtil Instance;

    private bool is_tutorial_end = false;
    private float start_time;

    private void Awake()
    {
        Instance = this;
    }

    private bool is_game_end = false;
    private void Update()
    {
        CounterCheck();
        FeverTimeCheck();
        if (game_over)
        {
            game_over = false;
            if (is_game_end) return;
            is_game_end = true;
            // Time.timeScale = 0;
            MapController.instance.speed = 0f;
            MapController.instance.is_game_over = true;

            game_over_img.SetActive(true);
            gameover_score_text.text = "SCORE: " + score.ToString();
            best_combo_text.text = "最高Combo: " + best_combo.ToString();
            survive_text.text = "你们在大圣手下活了" + Mathf.RoundToInt((Time.fixedTime - start_time) * 10f) * 0.1f + "秒！";

            if (best_combo < 10)
                achievement_text.text = "有待提高！";
            else if (best_combo < 30)
                achievement_text.text = "再接再厉！";
            else if (best_combo < 60)
                achievement_text.text = "游刃有余！";
            else if (best_combo < 90)
                achievement_text.text = "精彩绝伦！";
            else if (best_combo < 120)
                achievement_text.text = "出神入化！";
            else if (best_combo < 150)
                achievement_text.text = "登峰造极！";
            else if (best_combo >= 150)
                achievement_text.text = "国士无双！";

            gameover_score_text.transform.localScale = Vector3.zero;
            best_combo_text.transform.localScale = Vector3.zero;
            survive_text.transform.localScale = Vector3.zero;
            achievement_text.transform.localScale = Vector3.zero;
            gameover_score_text.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.InQuart);
            best_combo_text.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.InQuart).SetDelay(0.4f);
            survive_text.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.InQuart).SetDelay(0.8f);
            achievement_text.transform.DOScale(Vector3.one * 5, 0.01f).SetDelay(1.5f);
            achievement_text.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InQuart).SetDelay(1.51f);
        }
    }

    public void RestartGame()
    {
        DOVirtual.DelayedCall(0.1f, () =>
        {
            SceneManager.LoadScene(0);
        }).SetUpdate(true);
    }

    private void FeverTimeCheck()
    {
        if (counter < 40)
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
        if (!is_tutorial_end) return;
        if (Time.fixedTime - last_score_check_time < score_add_duration) return;
        last_score_check_time = Time.fixedTime;
        if (!hitted)
        {
            ++counter;
            score += counter / 60 + 1;
        }
        else
        {
            counter = 0;
        }

        hitted = false;
        score_text.text = score.ToString();
    }

    public void AddCounterP1(int value)
    {
        if (!is_tutorial_end) return;
        counter_p1 += value;
        score += (counter_p1) * 2;
        RefreshScore();
    }
    public void AddCounterP2(int value)
    {
        if (!is_tutorial_end) return;
        counter_p2 += value;
        score += (counter_p2) * 2;
        RefreshScore();
    }

    void RefreshScore()
    {
        score_text.text = score.ToString();
        score_text.transform.localScale = Vector3.one;
        score_text.transform.DOScale(Vector3.one * 1.5f, 0.05f).SetEase(Ease.InQuart);
        score_text.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InQuart).SetDelay(0.05f);
        combo_text_p1.text = counter_p1.ToString();
        combo_text_p1.transform.localScale = Vector3.one;
        combo_text_p1.transform.DOScale(Vector3.one * 1.5f, 0.05f).SetEase(Ease.InQuart);
        combo_text_p1.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InQuart).SetDelay(0.05f);
        combo_text_p2.text = counter_p2.ToString();
        combo_text_p2.transform.localScale = Vector3.one;
        combo_text_p2.transform.DOScale(Vector3.one * 1.5f, 0.05f).SetEase(Ease.InQuart);
        combo_text_p2.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InQuart).SetDelay(0.05f);
    }

    public void ResetCounterP1()
    {
        if (counter_p1 > best_combo) best_combo = counter_p1;
        counter_p1 = 0;
    }

    public void ResetCounterP2()
    {
        if (counter_p2 > best_combo) best_combo = counter_p1;
        counter_p2 = 0;
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
            if (health_still) return;
            if (value < p1_health)
            {
                hitted = true;
                PlayerManager.instance.SetP1Hurt();
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
            if (health_still) return;
            if (value < p2_health)
            {
                hitted = true;
                PlayerManager.instance.SetP2Hurt();
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

    public void TutorialEnd()
    {
        if (is_tutorial_end) return;
        is_tutorial_end = true;
        start_time = Time.fixedTime;
        P1Health = 100;
        P2Health = 100;
    }
}