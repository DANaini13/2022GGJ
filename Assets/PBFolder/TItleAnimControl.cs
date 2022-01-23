using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TItleAnimControl : MonoBehaviour
{
    public float rotateTime = 3;
    private void Start()
    {
        DOVirtual.DelayedCall(rotateTime,() => { SetItemRotate(); }).SetEase(Ease.Linear);
        
    }
    public void SetItemRotate()
    {
        transform.DOLocalRotate(new Vector3(0, 0, 180), 0, RotateMode.LocalAxisAdd).OnComplete(()=> {
            DOVirtual.DelayedCall(rotateTime, () => { SetItemRotate(); }).SetEase(Ease.Linear);
        });
    }
}
