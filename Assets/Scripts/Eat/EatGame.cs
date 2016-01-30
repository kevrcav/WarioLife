using UnityEngine;
using System.Collections;

public class EatGame : MonoBehaviour {

    public bool hasActed = false;
    public bool isActing = false;
    public Animator anchorAnim;
    public Animator handAnim;
    public Animator headAnim;
    public Animator bodyAnim;
    public float maxAnchorY = 0;
    public float minAnchorY = 0;
    MiniGame miniGame;

	// Use this for initialization
	void Start ()
    {
        miniGame = GetComponent<MiniGame>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.anyKeyDown)
        {
            if (!hasActed)
                ResolveAction();
//            else
//                ResetAction();
        }
	
	}

    // woo doesn't work
//    void ResetAction()
//    {
//        Debug.Log("Woo, lets reset!");
//        handAnim.ResetTrigger("result_too_low");
//        handAnim.ResetTrigger("result_too_high");
//        handAnim.ResetTrigger("result_just_right");
//        anchorAnim.ResetTrigger("result_just_right");
//        anchorAnim.StopPlayback();
//        anchorAnim.Play("hand_updown");
//        anchorAnim.StartPlayback();
//        handAnim.Play("hand_reset");
//        hasActed = false;
//    }

    void ResolveAction()
    {
        Debug.Log("WOO LET'S RESOLVE THIS ACTION");
        float anchor_y = anchorAnim.transform.localPosition.y;
        if (anchor_y < minAnchorY)
        {
            handAnim.SetTrigger("result_too_low");
            anchorAnim.Stop();
            headAnim.SetTrigger("result_fail_01");
            bodyAnim.SetTrigger("result_fail_01");
            miniGame.ReportLose();
        }
        else if (anchor_y > maxAnchorY)
        {
            handAnim.SetTrigger("result_too_high");
            anchorAnim.Stop();
            headAnim.SetTrigger("result_fail_01");
            bodyAnim.SetTrigger("result_fail_01");
            miniGame.ReportLose();
        }
        else
        {
            handAnim.SetTrigger("result_just_right");
            anchorAnim.SetTrigger("result_just_right");
            headAnim.SetTrigger("result_success_01");
            bodyAnim.SetTrigger("result_success_01");
            miniGame.ReportWin();
        }
        hasActed = true;
    }
}
