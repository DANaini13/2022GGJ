using UnityEngine;

public class PlayerMask : MonoBehaviour
{
    static public PlayerMask instance;
    private RectTransform trans;
    public SceneItemFollower mask;
    public ParticleSystem p1_black;
    public ParticleSystem p2_black;

    private void Awake()
    {
        instance = this;
        trans = GetComponent<RectTransform>();
    }

    public Transform p1_trans;
    public Transform p2_trans;

    void Update()
    {
        if (p1_black.isPlaying)
        {
            p1_timer -= Time.deltaTime;
            if (p1_timer <= 0f)
                p1_black.Stop();
        }

        if (p2_black.isPlaying)
        {
            p2_timer -= Time.deltaTime;
            if (p2_timer <= 0f)
                p2_black.Stop();
        }
    }

    float p1_timer, p2_timer;
    public void LightP1()
    {
        p1_black.Play();
        p2_black.Stop();
        p1_timer = 5f;
        p2_timer = 0f;
    }

    public void LightP2()
    {
        p2_black.Play();
        p1_black.Stop();
        p2_timer = 5f;
        p1_timer = 0f;
    }

    public void ChangeLight()
    {
        if (mask.following == p1_trans)
        {
            trans.localScale = new Vector3(1, -1, 1);
            mask.camera_id = 1;
            CameraUtil.GetCameraById(0).depth = 0;
            CameraUtil.GetCameraById(1).depth = 1;
            p2_black.Play();
            p1_black.Stop();
        }
        else
        {
            trans.localScale = new Vector3(1, 1, 1);
            mask.camera_id = 0;
            CameraUtil.GetCameraById(0).depth = 1;
            CameraUtil.GetCameraById(1).depth = 0;
            p1_black.Play();
            p2_black.Stop();
        }
        mask.following = mask.following == p1_trans ? p2_trans : p1_trans;
    }

    public void Invert()
    {
        (p1_trans, p2_trans) = (p2_trans, p1_trans);
        mask.following = mask.following == p1_trans ? p1_trans : p2_trans;
        mask.camera_id = mask.following == p1_trans ? 0 : 1;
        if (mask.following == p1_trans)
        {
            CameraUtil.GetCameraById(0).depth = 1;
            CameraUtil.GetCameraById(1).depth = 0;
        }
        else
        {
            CameraUtil.GetCameraById(0).depth = 0;
            CameraUtil.GetCameraById(1).depth = 1;
        }
    }
}
