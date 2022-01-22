using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private ParticleSystem stop_particle;
    public AudioClip step_audio;
    public AudioSource audio_s;
    public ParticleSystem jump_over_particle;
    public AnimatorEvent animator_event;
    private BoxCollider box_collider;
    public enum PlayerIdType {P1, P2}
    public PlayerIdType player_id;
    public KeyCode jump_key;
    public KeyCode squat_key;
    public KeyCode catch_key;
    public KeyCode attack_key;
    public AnimationCurve jump_curve;
    public float jump_time = 0.5f;
    public float jump_height = 1;
    public float jump_to_squat_time = 0.2f;
    public float squat_time = 0.5f;
    public float attack_time = 0.5f;
    public float catching_time = 1.0f;
    public bool catched = false;
    public PlayerControl catching_player = null;
    public Vector3 catching_offset = new Vector3(-0.5f, 0, 0);
    private Animator animator;

    private void Awake()
    {
        box_collider = GetComponent<BoxCollider>();
        idle_pos = transform.position;
        animator = transform.GetChild(0).GetComponent<Animator>();
        stop_particle = Resources.Load<ParticleSystem>("Prefabs/VFX_StepEffect");
        animator_event.on_clip_finished = clipName =>
        {
            var ps = Instantiate(stop_particle, transform);
            ps.transform.position = transform.position + new Vector3(0, 0.01f, 0);
            ps.Play();
            stop_particle.Play();
        };
    }

    public Vector3 idle_pos = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (catched) return;
        CheckCallWalk();
        ListenInput();
        if (jumping)
        {
            float current_index = (Time.fixedTime - jump_start_time)/jump_time;
            transform.position = new Vector3(idle_pos.x, jump_curve.Evaluate(current_index) * jump_height + idle_pos.y, idle_pos.z);
            if (current_index >= 1)
            {
                jumping = false;
                transform.position = idle_pos;
                jump_over_particle.Play();
            }
        }
        else if(jump_to_squat)
        {
        }
        else
        {
            transform.position = idle_pos;
        }

        if (catching_player != null)
        {
            catching_player.transform.position = transform.position + catching_offset;
        }
    }

    private float step_audio_cd = 0.1f;
    private float last_stop_audio_time = 0;
    private float last_walk_time = 0;
    private void CheckCallWalk()
    {
        if (jumping || squating) return;
        float walk_duration = 1.0f / MapController.instance.speed;
        if (Time.fixedTime - last_walk_time < walk_duration) return;
        float real_speed = MapController.instance.speed > 19 ? 19 : MapController.instance.speed;
        if (Time.fixedTime - last_stop_audio_time > step_audio_cd)
        {
            audio_s.PlayOneShot(step_audio);
            last_stop_audio_time = Time.fixedTime;
        }
        animator.SetFloat("speed", real_speed/2.0f);
        animator.SetTrigger("walk");
        last_walk_time = Time.fixedTime;
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
        }else if (Input.GetKeyDown(catch_key))
        {
            Catch();
        }else if (Input.GetKeyDown(attack_key))
        {
            Attack();
        }
    }

    private bool jumping = false;
    private bool squating = false;
    private float jump_start_time = 0;
    public void Jump()
    {
        if (jumping) return;
        jumping = true;
        animator.SetFloat("jumpSpeed", 1.0f/jump_time * 0.5f);
        animator.SetTrigger("jump");
        var pos_list = new List<Vector3>();
        var temp_pos = transform.position;
        jump_start_time = Time.fixedTime;
    }

    private bool jump_to_squat = false;
    public void Squat()
    {
        if (jumping)
        {
            if (squating) transform.DOKill();
            transform.DOMove(idle_pos, jump_to_squat_time).SetEase(Ease.Linear).onComplete = DoSquat;
            jump_to_squat = true;
            jumping = false;
        }
        else
        {
            if (squating) return;
            DoSquat();
        }
    }

    public void Catch()
    {
        if (catching_player != null) return;
        if (player_id == PlayerIdType.P1)
        {
            catching_player = PlayerManager.instance.player_2;
        }
        else
        {
            catching_player = PlayerManager.instance.player_1;
        }
        catching_player.catched = true;
        DOVirtual.DelayedCall(catching_time, () =>
        {
            catching_player.catched = false;
            catching_player = null;
        });
    }

    private bool attacking = false;
    public void Attack()
    {
        if (attacking) return;
        attacking = true;
        animator.SetFloat("hitSpeed", 1.0f/attack_time * 0.5f);
        animator.SetTrigger("hit");
        // 检测攻击距离内的方块
        int layer = 1 << LayerMask.NameToLayer("breakable");
        //射线中心点(就是球的中心点) 半径 层级 一般技能用这个
        Collider[] cols = Physics.OverlapSphere(transform.position, 3.5f, layer);
        for (int i = 0; i < cols.Length; i++)
        {
            if (!cols[i].CompareTag("breakable"))
            {
                continue;
            }
            if(cols[i].transform.position.x < transform.position.x) continue;
            cols[i].GetComponent<BreakableCube>().OnBreak();
            break;
        }
        DOVirtual.DelayedCall(attack_time, () =>
        {
            attacking = false;
        });
    }

    private void DoSquat()
    {
        squating = true;
        jump_to_squat = false;
        animator.SetFloat("crouchSpeed", 1.0f/squat_time * 0.5f);
        animator.SetTrigger("crouch");
        var temp_size = box_collider.size;
        var original_size = box_collider.size;
        temp_size.y *= 0.5f;
        box_collider.size = temp_size;
        transform.DOKill();
        transform.DOScaleX(1, squat_time).onComplete = () =>
        {
            squating = false;
            box_collider.size = original_size;
        };
    }

    private float hurt_cd = 0.5f;
    public int hurt_amount = 1;
    private float last_hurt_time = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) return;
        if (other.gameObject.CompareTag("tool")) return;
        if (!other.gameObject.CompareTag("block") && !other.gameObject.CompareTag("breakable"))
        {
            return;
        }
        hurt_cd = 2.1f/MapController.instance.speed;
        if (Time.fixedTime - last_hurt_time < hurt_cd) return;
        if (player_id == PlayerIdType.P1)
            PlayerDataUtil.Instance.P1Health -= hurt_amount;
        else
            PlayerDataUtil.Instance.P2Health -= hurt_amount;
        last_hurt_time = Time.fixedTime;
    }
}
