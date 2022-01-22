using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class PostEffect : MonoBehaviour
{
    [HideInInspector] public Shader shader;
    Material materialInstance;
    protected Material material
    {
        get
        {
            if (shader == null) return null;
            if (materialInstance == null)
            {
                materialInstance = new Material(shader);
                materialInstance.hideFlags = HideFlags.DontSave;
            }

            return materialInstance;
        }
    }

    bool isShaderSupport
    {
        get
        {
            if (shader == null) return false;
            return shader.isSupported;
        }
    }
    Camera cameraInstance;
    protected Camera cameraComponent
    {
        get
        {
            if (cameraInstance == null)
            {
                cameraInstance = GetComponent<Camera>();
            }

            return cameraInstance;
        }
    }
}