using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    public bool isActive = false;
    public int count = 20;
    public Transform tilePrefab;

    void Awake()
    {
        if (!isActive) return;
        for (int i = 0; i < count; i++)
        {
            Transform prefab = Instantiate(tilePrefab);
            prefab.gameObject.SetActive(true);
            prefab.SetParent(this.transform);
            prefab.localPosition = Vector3.right * i;
        }
    }
}
