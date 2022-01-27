using UnityEngine;

public class Coin : SceneTool
{
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        other.GetComponent<PlayerControl>().PickCoin();
        PlayAudio();
        GameObject.Destroy(gameObject);
    }
}
