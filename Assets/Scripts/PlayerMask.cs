using UnityEngine;

public class PlayerMask : MonoBehaviour
{
    static public PlayerMask instance;
    private RectTransform trans;
    public SceneItemFollower mask;

    private void Awake()
    {
        instance = this;
        trans = GetComponent<RectTransform>();
    }
    
    public Transform p1_trans;
    public Transform p2_trans;

    private bool p1_mask = true;
    private void SetMaskP1()
    {
        if (p1_mask) return;
        trans.eulerAngles = Vector3.zero;
        trans.localScale = new Vector3(1, 1, 1);
        mask.camera_id = 0;
        mask.following = p1_trans;
        CameraUtil.GetCameraById(0).depth = 1;
        CameraUtil.GetCameraById(1).depth = 0;
        p1_mask = true;
    }

    private void SetMaskP2()
    {
        if (!p1_mask) return;
        trans.localScale = new Vector3(1, -1, 1);
        mask.camera_id = 1;
        mask.following = p2_trans;
        CameraUtil.GetCameraById(0).depth = 0;
        CameraUtil.GetCameraById(1).depth = 1;
        p1_mask = false;
    }

    public void ChangeLight()
    {
        if(p1_mask) SetMaskP2();
        else SetMaskP1();
    }
}
