using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private BoxCollider box_collider;
    public KeyCode jump_key;
    public KeyCode squat_key;
    public AnimationCurve jump_curve;
    public float jump_time = 0.5f;
    public float jump_height = 1;
    public float squat_time = 0.5f;

    private void Awake()
    {
        box_collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        ListenInput();
    }

    private void ListenInput()
    {
        if (!Input.anyKeyDown)
            return;
        if (Input.GetKeyDown(jump_key))
        {
            Jump();
        }else if (Input.GetKeyDown(squat_key))
        {
            Squat();
        }
    }

    private bool jumping = false;
    private bool squating = false;
    public void Jump()
    {
        if (jumping == true) return;
        jumping = true;
        var pos_list = new List<Vector3>();
        var temp_pos = transform.position;
        for (int i = 0; i <= 100; ++i)
        {
            var pos = transform.position;
            float height = jump_curve.Evaluate(i / 100.0f) * jump_height + pos.y;
            pos.y = height;
            pos_list.Add(pos);
        }
        transform.DOPath(pos_list.ToArray(), jump_time).onComplete = () =>
        {
            jumping = false;
            transform.position = temp_pos;
        };
    }

    public void Squat()
    {
        if (squating) return;
        squating = true;
        var temp_size = box_collider.size;
        var original_size = box_collider.size;
        temp_size.y *= 0.5f;
        box_collider.size = temp_size;
        DOVirtual.DelayedCall(squat_time, () =>
        {
            squating = false;
            box_collider.size = original_size;
        }).SetUpdate(false);
    }
}
