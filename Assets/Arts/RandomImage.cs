using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomImage : MonoBehaviour
{
    public MeshRenderer[] mrs;
    public Texture[] texs;


    void Awake()
    {
        var block = new MaterialPropertyBlock();
        block.SetTexture("_PropTex", texs[Random.Range(0, texs.Length)]);
        for (int i = 0; i < mrs.Length; i++)
            mrs[i].SetPropertyBlock(block);
    }
}
