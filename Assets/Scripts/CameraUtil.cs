using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraUtil : MonoBehaviour
{
    public Camera camera_0;
    public Camera camera_1;

    static private CameraUtil instance;

    private void Awake()
    {
        instance = this;
    }

    static public Camera GetCameraById(int id)
    {
        return id == 0 ? instance.camera_0 : instance.camera_1;
    }
}
