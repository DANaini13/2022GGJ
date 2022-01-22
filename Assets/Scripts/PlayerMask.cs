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
        mask.camera_id = 0;
        mask.following = p1_trans;
        p1_mask = true;
    }

    private void SetMaskP2()
    {
        if (!p1_mask) return;
        trans.eulerAngles = new Vector3(180, 0, 0);
        mask.camera_id = 1;
        mask.following = p2_trans;
        p1_mask = false;
    }

    public void ChangeLight()
    {
        if(p1_mask) SetMaskP2();
        else SetMaskP1();
    }
}
