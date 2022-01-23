using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraAnimControl : MonoBehaviour
{

    public Vector3 defaultRotate;
    [Header("相机旋转强度")]
    public float strength = 10;

    [Header("时长")]
    public float time = 5;


    public void PlayCam()
    {
        defaultRotate = transform.localEulerAngles;

        SetCamAnim();
    }

    public void SetCamAnim()
    {
        transform.DOLocalRotate(new Vector3(Random.Range(-strength, strength), Random.Range(-strength, strength), Random.Range(-strength, strength)), time, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).OnComplete(()=> {
            transform.DOLocalRotate(defaultRotate, time).SetEase(Ease.Linear).OnComplete(()=> {
                SetCamAnim();

            });
        });
    }
}
