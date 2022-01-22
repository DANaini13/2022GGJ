using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraAnimControl : MonoBehaviour
{

    public Vector3 defaultRotate;


    void Start()
    {
        defaultRotate = transform.localEulerAngles;

        SetCamAnim();
    }

    public void SetCamAnim()
    {
        transform.DOLocalRotate(new Vector3(Random.Range(-10, 10f), Random.Range(-10, 10f), Random.Range(-10, 10f)), 5, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).OnComplete(()=> {
            transform.DOLocalRotate(defaultRotate, 5).SetEase(Ease.Linear).OnComplete(()=> {
                SetCamAnim();

            });
        });
    }
}
