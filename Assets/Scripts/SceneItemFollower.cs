using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItemFollower : MonoBehaviour
{
    public Transform following = null;
    public int camera_id = 0;

    void Update()
    {
        if (following == null) return;
        transform.position = CameraUtil.GetCameraById(camera_id).WorldToScreenPoint(following.position);
    }
}
