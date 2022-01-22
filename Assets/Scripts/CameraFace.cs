using UnityEngine;

public class CameraFace : MonoBehaviour
{
    public int camera_id = 0;

    void Update()
    {
        var temp = transform.eulerAngles;
        transform.LookAt(CameraUtil.GetCameraById(camera_id).transform);
        var current_el = transform.eulerAngles;
        current_el.x = temp.x;
        current_el.z = temp.z;
        transform.eulerAngles = current_el;
    }
}
