using System;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private GameObject explod_cube = null;

    public void Awake()
    {
        explod_cube = Resources.Load("Prefabs/DoNotChange/VFX_Cube_Clap") as GameObject;
    }

    public void Break()
    {
        for (int i = 0; i < 2; i++)
        {
            foreach (Transform child in transform.GetChild(i))
            {
                var pos = child.position;
                var fx = Instantiate(explod_cube, transform).GetComponent<Explod>();
                fx.transform.position = pos;
            }
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }
}
