using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTool : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        GameObject.Destroy(gameObject);
        PlayerMask.instance.ChangeLight();
    }
}
