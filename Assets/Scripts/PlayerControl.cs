using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    private ParticleSystem stop_particle;
    public AudioClip step_audio;
    public AudioClip grab_audio;
    public AudioSource audio_s;
    public ParticleSystem jump_over_particle;
    public ParticleSystem hit_ps;
    public AnimatorEvent animator_event;
    private BoxCollider box_collider;
    public enum PlayerIdType { P1, P2 }
    public PlayerIdType player_id;
    public KeyCode jump_key;
    public KeyCode squat_key;
    public KeyCode catch_key;
    public KeyCode attack_key;
    public AnimationCurve jump_curve;
    private float jump_time;
    public float jump_time_default = 1f;
    public float jump_buffer_multiplier = 0.2f;//跳跃时间还剩余多少百分比时可以再次跳跃
    public float jump_height = 5;
    public float jump_to_squat_time = 0.05f;
    private float squat_time;
    public float squat_time_default = 1f;
    public float squat_buffer_multiplier = 0.2f;//下蹲时间还剩余多少百分比时可以再次下蹲
    private float attack_time;
    public float attack_time_default = 1f;
    public float attack_buffer_multiplier = 0.2f;//再攻击缓冲
    private float catching_time;
    public float catching_time_default = 1.0f;
    public bool catched = false;
    public PlayerControl catching_player = null;
    public Vector3 catching_offset = new Vector3(-0.5f, 0, 0);
    public ParticleSystem ps_pass_check_point;
    private Animator animator;
    private float squat_timer, jump_timer, attack_timer;
    private Vector3 original_collider_size;

    private void Awake()
    {
        jump_time = jump_time_default;
        squat_time = squat_time_default;
        attack_time = attack_time_default;
        catching_time = catching_time_default;
        box_collider = GetComponent<BoxCollider>();
        original_collider_size = box_collider.size;//在初始化时记录，避免下蹲期间多次下蹲导致尺寸错误
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
            float current_index = (Time.fixedTime - jump_start_time) / jump_time;
            transform.position = new Vector3(idle_pos.x, jump_curve.Evaluate(current_index) * jump_height + idle_pos.y, idle_pos.z);
            if (current_index >= 1)
            {
                jumping = false;
                transform.position = idle_pos;
                jump_over_particle.Play();
            }
        }
        else if (jump_to_squat)
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

        if (squating) squat_timer += Time.deltaTime;
        if (jumping) jump_timer += Time.deltaTime;
        if (attacking) attack_timer += Time.deltaTime;

        //刷新跳跃和下蹲速度
        squat_time = (1.0f - MapController.instance.GetCurDifficulty() / 130.0f) * squat_time_default;
        jump_time = (1.0f - MapController.instance.GetCurDifficulty() / 130.0f) * jump_time_default;
        attack_time = (1.0f - MapController.instance.GetCurDifficulty() / 130.0f) * attack_time_default;
        catching_time = (1.0f - MapController.instance.GetCurDifficulty() / 130.0f) * catching_time_default;
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
        animator.SetFloat("speed", real_speed / 2.0f);
        animator.SetTrigger("walk");
        last_walk_time = Time.fixedTime;
        // 射线检测下方的block
        // RaycastHit hit;
        // bool grounded = Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), Vector3.down, out hit, 3, 1<<LayerMask.NameToLayer("Tile"));
        // if (grounded)
        // {
        //     Debug.Log(hit.collider.gameObject.transform.position);
        // }
    }

    private void ListenInput()
    {
        if (!Input.anyKeyDown)
            return;
        if (Input.GetKeyDown(jump_key))
        {
            Jump();
        }
        else if (Input.GetKeyDown(squat_key))
        {
            Squat();
        }
        else if (Input.GetKeyDown(catch_key))
        {
            Catch();
        }
        else if (Input.GetKeyDown(attack_key))
        {
            Attack();
        }
    }

    private bool jumping = false;
    private bool squating = false;
    private float jump_start_time = 0;
    public void Jump()
    {
        if (jumping && jump_timer < (jump_time * (1.0f - jump_buffer_multiplier))) return;
        jumping = true;
        jump_timer = 0f;
        animator.SetFloat("jumpSpeed", 1.0f / jump_time * 0.5f);
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
            if (squating && squat_timer < (squat_time * (1.0f - squat_buffer_multiplier))) return;
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
        audio_s.PlayOneShot(grab_audio);
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
        if (attacking && attack_timer < attack_time * (1.0f - attack_buffer_multiplier)) return;
        attacking = true;
        attack_timer = 0f;
        animator.SetFloat("hitSpeed", 1.0f / attack_time * 0.5f);
        animator.SetTrigger("hit");
        // 检测攻击距离内的方块
        int layer = 1 << LayerMask.NameToLayer("breakable");
        //射线中心点(就是球的中心点) 半径 层级 一般技能用这个
        Collider[] cols = Physics.OverlapSphere(transform.position, 5f, layer);
        for (int i = 0; i < cols.Length; i++)
        {
            if (!cols[i].CompareTag("breakable"))
            {
                continue;
            }
            if (cols[i].transform.position.x < transform.position.x) continue;
            cols[i].GetComponent<BreakableCube>().OnBreak();
            var ps = Instantiate(hit_ps, transform);
            ps.Play();
            break;
        }
        DOVirtual.DelayedCall(attack_time, () =>
        {
            attacking = false;
        });
    }

    public void OnHurt()
    {
        animator.SetTrigger("hurt");
        MapController.instance.SlowDown();//受伤后降低速度
    }

    private void DoSquat()
    {
        squating = true;
        squat_timer = 0f;
        jump_to_squat = false;
        animator.SetFloat("crouchSpeed", 1.0f / squat_time * 0.5f);
        animator.SetTrigger("crouch");
        var temp_size = original_collider_size;
        temp_size.y *= 0.5f;
        box_collider.size = temp_size;
        transform.DOKill();
        transform.DOScaleX(1, squat_time).onComplete = () =>
        {
            squating = false;
            box_collider.size = original_collider_size;
        };
    }

    private float hurt_cd = 0.5f;
    public int hurt_amount = 1;
    private float last_hurt_time = 0;
    private bool can_pass_check_point;
    private float last_pass_cp_time;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) return;
        if (other.gameObject.CompareTag("tool")) return;
        if (other.gameObject.CompareTag("startBlock"))
        {
            PlayerDataUtil.Instance.TutorialEnd();
            MapController.instance.TutorialEnd();
        }
        if (other.gameObject.CompareTag("checkPoint"))
        {
            if (Time.fixedTime - last_pass_cp_time < 0.25f) can_pass_check_point = false;//距离上次离开检测点时间太短时，关闭判断（通常出现在吸附结束时又进入了判定点）
        }
        BlockAnim bAnim = other.gameObject.GetComponent<BlockAnim>();
        if (other.gameObject.CompareTag("catchBlock"))
        {
            if (bAnim)
                bAnim.Rotate();
        }

        if (!other.gameObject.CompareTag("block") && !other.gameObject.CompareTag("breakable"))
        {
            return;
        }

        if (bAnim)
            bAnim.Break();
        can_pass_check_point = false;
        hurt_cd = 0.5f;
        if (Time.fixedTime - last_hurt_time < hurt_cd) return;
        if (player_id == PlayerIdType.P1)
        {
            PlayerDataUtil.Instance.P1Health -= hurt_amount;
            PlayerDataUtil.Instance.ResetCounterP1();
        }
        else
        {
            PlayerDataUtil.Instance.P2Health -= hurt_amount;
            PlayerDataUtil.Instance.ResetCounterP2();
        }
        last_hurt_time = Time.fixedTime;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("checkPoint")) return;
        last_pass_cp_time = Time.fixedTime;
        if (can_pass_check_point)
        {
            if (player_id == PlayerIdType.P1)
            {
                PlayerDataUtil.Instance.AddCounterP1(5);
                Instantiate(ps_pass_check_point).transform.position = this.transform.position;
            }
            else
            {
                PlayerDataUtil.Instance.AddCounterP2(5);
                Instantiate(ps_pass_check_point).transform.position = this.transform.position;
            }
        }
        else
            can_pass_check_point = true;
    }

    public void PickCoin()
    {
        if (player_id == PlayerIdType.P1)
            PlayerDataUtil.Instance.AddCounterP1(1);
        else
            PlayerDataUtil.Instance.AddCounterP2(1);
    }
}
