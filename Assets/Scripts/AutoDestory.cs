using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestory : MonoBehaviour
{
    public float destroy_after = 5.0f;
    void Start()
    {
        GameObject.Destroy(gameObject, destroy_after);
    }

}
