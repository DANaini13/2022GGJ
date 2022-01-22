using System;
using UnityEditor.iOS;
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
        var child_count = transform.childCount;
        for (int i = 0; i < child_count; i++)
        {
            var child = transform.GetChild(i);
            var pos = child.position;
            var fx = Instantiate(explod_cube, transform).GetComponent<Explod>();
            fx.transform.position = pos;
            GameObject.Destroy(child.gameObject);
        }
    }
}
