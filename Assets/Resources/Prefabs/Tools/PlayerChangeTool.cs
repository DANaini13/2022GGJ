using UnityEngine;

public class PlayerChangeTool : SceneTool
{
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        PlayAudio();
        PlayerManager.instance.SwapPlayer();
        PlayerMask.instance.Invert();
        GameObject.Destroy(gameObject);
    } 
}
