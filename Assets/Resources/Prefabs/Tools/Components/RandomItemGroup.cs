using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemGroup : MonoBehaviour
{
    void Start()
    {
        List<Transform> list = new List<Transform>();
        foreach (Transform child in this.transform)
        {
            list.Add(child);
        }
        list.RemoveAt(Random.Range(0, list.Count));
        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i].gameObject);
        }
    }
}
