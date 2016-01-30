using UnityEngine;
using System.Collections;

public class EatGame : MonoBehaviour {

    private bool hasActed = false;
    private bool isActing = false;
    [Range(0,1)]
    public float difficulty = 0;
    public float minAnimSpeed = 1;
    public float maxAnimSpeed = 3;
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
        OneButtonInputMgr.Instance.OnButtonDown += OnButtonDown;
        SetupMiniGame();
	}
	
    void OnDisable()
    {
        OneButtonInputMgr.Instance.OnButtonDown -= OnButtonDown;
    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.anyKeyDown)
        {
//            if (!hasActed)
//                ResolveAction();
//            else
//                ResetAction();
        }
	
	}

    void OnButtonDown()
    {
        if (!hasActed)
            ResolveAction();
    }

    void SetupMiniGame()
    {
        SetSpeed();
    }
 
    void SetSpeed()
    {
        float speed = Mathf.Lerp(minAnimSpeed, maxAnimSpeed, difficulty);
        handAnim.speed = speed;
        bodyAnim.speed = speed;
        headAnim.speed = speed;
        anchorAnim.speed = speed;
    }

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
            miniGame.ReportWin(1.5f);
        }
        hasActed = true;
    }
}
