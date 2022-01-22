using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmplifyColor;

[RequireComponent(typeof(AmplifyColorEffect))]
public class DayNight : MonoBehaviour
{
    public bool isDay = true;
    public float durationTime = 5f;
    public float lerpTime = 2f;
    private bool isLerping = false;
    private float durationTimer, lerpTimer;
    AmplifyColorEffect ac;

    void Awake()
    {
        ac = this.GetComponent<AmplifyColorEffect>();
        ac.LutTexture = Resources.Load<Texture>("AmplifyColorTex/Day");
        ac.LutBlendTexture = Resources.Load<Texture>("AmplifyColorTex/Night");
        if (!isDay)
            ac.BlendAmount = 1;
    }

    void Update()
    {
        if (!isLerping)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer >= durationTime)
                isLerping = true;
        }

        else
        {
            lerpTimer += Time.deltaTime;
            ac.BlendAmount = isDay ? lerpTimer / lerpTime : (1 - lerpTimer / lerpTime);
            if (lerpTimer >= lerpTime)
            {
                ac.BlendAmount = isDay ? 1 : 0;
                isDay = !isDay;
                lerpTimer = 0f;
                durationTimer = 0f;
                isLerping = false;
            }
        }
    }
}
