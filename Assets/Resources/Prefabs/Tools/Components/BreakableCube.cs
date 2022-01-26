
using UnityEngine;

public class BreakableCube : MonoBehaviour
{
    public void OnBreak()
    {
        transform.parent.GetComponent<Breakable>().Break();
    }
}
