using UnityEngine;

public class CameraFace : MonoBehaviour
{
    private Camera main_camera;
    void Start()
    {
        main_camera = Camera.main;
    }

    void Update()
    {
        var temp = transform.eulerAngles;
        transform.LookAt(main_camera.transform);
        var current_el = transform.eulerAngles;
        current_el.x = temp.x;
        current_el.z = temp.z;
        transform.eulerAngles = current_el;
    }
}
