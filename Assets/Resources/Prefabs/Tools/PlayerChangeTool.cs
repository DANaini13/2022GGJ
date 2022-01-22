using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeTool : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        GameObject.Destroy(gameObject);
        PlayerManager.instance.SwapPlayer();
        PlayerMask.instance.Invert();
    } 
}
