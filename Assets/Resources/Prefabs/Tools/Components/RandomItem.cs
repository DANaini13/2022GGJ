using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    public Transform[] itemPrefabs;
    public int tangSengIndex;
    public int chance = 20;

    void Start()
    {
        if (Random.Range(0, 100) < chance)
        {
            int index = Random.Range(0, itemPrefabs.Length);
            if (PlayerDataUtil.Instance.P1Health < 50 || PlayerDataUtil.Instance.P2Health < 50)
                if (Random.Range(0, 100) < 50) index = tangSengIndex;
            Transform prefab = Instantiate(itemPrefabs[index], this.transform.parent).transform;
            prefab.position = this.transform.position;
        }
        Destroy(this.gameObject);
    }
}
