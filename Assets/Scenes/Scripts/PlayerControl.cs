using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public KeyCode jump_key;
    public KeyCode squat_key;
    public AnimationCurve jump_curve;
    public float jump_time = 0.5f;
    public float jump_height = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
        Debug.Log(jump_curve.Evaluate(0.3f));
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
        squating = true;
    }
}
