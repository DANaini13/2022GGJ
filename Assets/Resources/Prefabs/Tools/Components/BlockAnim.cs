using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockAnim : MonoBehaviour
{
    int state = 0;
    void Start()
    {
        // transform.localScale = Vector3.zero;
        // transform.Translate(Vector3.down * 1f);
    }

    void Update()
    {
        if (state == 0 && transform.position.x < -0.5f)
            Play();
    }

    void Play()
    {
        state = 2;
        float time = 1.2f - (MapController.instance.GetCurDifficulty() / 100.0f) * 1f;
        transform.DOMoveY(transform.position.y - 5f, time).SetEase(Ease.InQuart);
        transform.DOScale(Vector3.zero, time).SetEase(Ease.InQuart);
    }
}
