using UnityEngine;

public class LightTool : SceneTool
{
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        PlayAudio();
        // PlayerMask.instance.ChangeLight();
        if (this.transform.position.y > 0)
            PlayerMask.instance.LightP2();
        else
            PlayerMask.instance.LightP1();
        GameObject.Destroy(gameObject);
    }
}
