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

    public void ChangeLight()
    {
        if (mask.following == p1_trans)
        {
            trans.localScale = new Vector3(1, -1, 1);
            mask.camera_id = 1;
            CameraUtil.GetCameraById(0).depth = 0;
            CameraUtil.GetCameraById(1).depth = 1;
        }
        else
        {
            trans.localScale = new Vector3(1, 1, 1);
            mask.camera_id = 0;
            CameraUtil.GetCameraById(0).depth = 1;
            CameraUtil.GetCameraById(1).depth = 0;
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
