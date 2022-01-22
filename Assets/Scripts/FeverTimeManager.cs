using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FeverTimeManager: MonoBehaviour
{
    public List<Camera> camera_list;
    private List<Color> camera_bg_color_list = new List<Color>();
    public List<GameObject> scene_effects;
    private List<bool> scene_effect_status = new List<bool>();

    public static FeverTimeManager instance;

    public void Awake()
    {
        instance = this;
    }

    private bool fever_time = false;
    
    public void EnterFeverTime()
    {
        if (fever_time) return;
        fever_time = true;
        // 记录开关状态, 把开的都关了
        scene_effect_status.Clear();
        foreach (var scene_effect in scene_effects)
        {
            scene_effect_status.Add(scene_effect.activeSelf);
            scene_effect.SetActive(false);
        }
        // 开启特效shader
        float dissolve_amount = 1;
        transform.DOScaleX(1, 0.05f).SetLoops(32).onStepComplete = () =>
        {
            Shader.SetGlobalFloat("_AlphaPower", dissolve_amount);
            dissolve_amount = dissolve_amount < 0 ? 0 : dissolve_amount;
            dissolve_amount = dissolve_amount > 1 ? 1 : dissolve_amount;
            //Debug.Log(dissolve_amount);
            dissolve_amount -= 0.0334f;
        };
        // 缓存相机的bg颜色，设置相机的bg颜色到黑色
        camera_bg_color_list.Clear();
        foreach (var camera in camera_list)
        {
            camera_bg_color_list.Add(camera.backgroundColor);
            camera.backgroundColor = Color.black;
        }
    }

    public void ExitFeverTime()
    {
        if (!fever_time) return;
        fever_time = false;
        // 根据场景状态还原特效
        int index = 0;
        foreach (var scene_effect in scene_effects)
        {
            scene_effect.SetActive(scene_effect_status[index]);
            ++index;
        }
        // 关闭特效shader
        float dissolve_amount = 0;
        transform.DOScaleX(1, 0.05f).SetLoops(32).onStepComplete = () =>
        {
            Shader.SetGlobalFloat("_AlphaPower", dissolve_amount);
            dissolve_amount = dissolve_amount < 0 ? 0 : dissolve_amount;
            dissolve_amount = dissolve_amount > 1 ? 1 : dissolve_amount;
            //Debug.Log(dissolve_amount);
            dissolve_amount += 0.0334f;
        };
        // 恢复相机bg颜色
        index = 0;
        foreach (var camera in camera_list)
        {
            camera.backgroundColor = camera_bg_color_list[index];
            ++index;
        }
    }

    public void ChangeFeverTime()
    {
        if (fever_time)
        {
            ExitFeverTime();
        }
        else
        {
            EnterFeverTime();
        }
    }
}