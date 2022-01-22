using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabArrays
{
    public string desc;
    public Vector3 pos;
    public Vector3 posOffset;
    public Vector3 scale_min = Vector3.one;
    public Vector3 scale_max = Vector3.one;
    public float between_min, between_max;
    public Transform[] Array;
    public Transform this[int index]
    {
        get
        {
            return Array[index];
        }
    }
    public PrefabArrays()
    {
        this.Array = new Transform[4];
    }
    public PrefabArrays(int index)
    {
        this.Array = new Transform[index];
    }
}

public class SceneGenerator : MonoBehaviour
{
    public PrefabArrays[] prefabsArrays;
    public float maxDistance = 20f;

    void Awake()
    {
        for (int i = 0; i < prefabsArrays.Length; i++)
        {
            Vector3 lastPos = Vector3.zero;
            while (true)
            {
                Transform prefab = Instantiate(prefabsArrays[i].Array[Random.Range(0, prefabsArrays[i].Array.Length)]).transform;
                prefab.SetParent(this.transform);
                if (lastPos.Equals(Vector3.zero))
                    lastPos = prefabsArrays[i].pos;
                lastPos.x += Random.Range(prefabsArrays[i].between_min, prefabsArrays[i].between_max);
                Vector3 pos = lastPos;
                pos.x += Random.Range(-prefabsArrays[i].posOffset.x, prefabsArrays[i].posOffset.x);
                pos.y += Random.Range(-prefabsArrays[i].posOffset.y, prefabsArrays[i].posOffset.y);
                pos.z += Random.Range(-prefabsArrays[i].posOffset.z, prefabsArrays[i].posOffset.z);
                prefab.localPosition = pos;
                Vector3 scale_min = prefabsArrays[i].scale_min;
                Vector3 sclae_max = prefabsArrays[i].scale_max;
                prefab.localScale = new Vector3(Random.Range(scale_min.x, sclae_max.x), Random.Range(scale_min.y, sclae_max.y), Random.Range(scale_min.z, sclae_max.z));

                if (lastPos.x >= maxDistance)
                    break;
            }
        }
    }
}
