using UnityEngine;

public class LightTool : SceneTool
{
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        PlayAudio();
        PlayerMask.instance.ChangeLight();
        GameObject.Destroy(gameObject);
    }
}
