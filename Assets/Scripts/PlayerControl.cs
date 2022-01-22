using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private BoxCollider box_collider;
    public enum PlayerIdType {P1, P2}
    public PlayerIdType player_id;
    public KeyCode jump_key;
    public KeyCode squat_key;
    public AnimationCurve jump_curve;
    public float jump_time = 0.5f;
    public float jump_height = 1;
    public float squat_time = 0.5f;

    private void Awake()
    {
        box_collider = GetComponent<BoxCollider>();
        idle_pos = transform.position;
    }

    private Vector3 idle_pos = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        ListenInput();
        if (jumping)
        {
            float current_index = (Time.fixedTime - jump_start_time)/jump_time;
            transform.position = new Vector3(idle_pos.x, jump_curve.Evaluate(current_index) * jump_height + idle_pos.y, idle_pos.z);
            if (current_index >= 1)
            {
                jumping = false;
                transform.position = idle_pos;
            }
        }
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
    private float jump_start_time = 0;
    public void Jump()
    {
        if (jumping) return;
        jumping = true;
        var pos_list = new List<Vector3>();
        var temp_pos = transform.position;
        jump_start_time = Time.fixedTime;
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
