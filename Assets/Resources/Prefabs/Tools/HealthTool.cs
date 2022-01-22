using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTool : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        GameObject.Destroy(gameObject);
        var pc = other.gameObject.GetComponent<PlayerControl>();
        if (pc.player_id == PlayerControl.PlayerIdType.P1)
            PlayerDataUtil.Instance.P1Health += 5;
        else
            PlayerDataUtil.Instance.P2Health += 5;
    } 
}
