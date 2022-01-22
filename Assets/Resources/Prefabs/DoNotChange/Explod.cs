using UnityEngine;
using Random = UnityEngine.Random;

public class Explod : MonoBehaviour
{
    private Rigidbody[] childs;
    private void Awake()
    {
        childs = GetComponentsInChildren<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var rb in childs)
        {
            rb.AddExplosionForce(300f, transform.position, 10);
        }
        GameObject.Destroy(gameObject, 5);
    }
}
