using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomImage : MonoBehaviour
{
    public Sprite[] sprites;

    void Awake()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
