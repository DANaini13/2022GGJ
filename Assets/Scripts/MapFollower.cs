using UnityEngine;

public class MapFollower : MonoBehaviour
{
    private Vector3 offset = Vector3.zero;
    private void Start()
    {
        offset = transform.position - MapController.instance.transform.position;
    }

    private void Update()
    {
        transform.position = MapController.instance.transform.position + offset;
    }
}
