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

    public void Break()
    {
        // if (!this.gameObject.GetComponent<Renderer>().enabled) return;
        // GameObject explod_cube = Resources.Load("Prefabs/DoNotChange/VFX_CubeDestroy") as GameObject;
        // var pos = this.transform.position;
        // var fx = Instantiate(explod_cube);
        // fx.transform.position = pos;
        // this.gameObject.GetComponent<Renderer>().enabled = false;//只关闭渲染，避免吸附时被玩家1撞破后，玩家2不会受到伤害判定
        // transform.DORotate(Vector3.forward * -720f, 2f).SetEase(Ease.OutQuart);
    }
    public void Rotate()
    {
        transform.DORotate(Vector3.forward * -720f, 2f).SetEase(Ease.OutQuart);
    }
}
