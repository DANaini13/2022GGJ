
using UnityEngine;

public class BreakableCube : MonoBehaviour
{
    public void OnBreak()
    {
        transform.parent.parent.GetComponent<Breakable>().Break();
    }
}
