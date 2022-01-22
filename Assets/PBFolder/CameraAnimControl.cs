using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraAnimControl : MonoBehaviour
{

    public Vector3 defaultRotate;
    [Header("�����תǿ�ȣ�")]
    public float strength = 10;

    [Header("ʱ����")]
    public float time = 5;


    void Start()
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
