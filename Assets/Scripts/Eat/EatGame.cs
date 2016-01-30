using UnityEngine;
using System.Collections;

public class EatGame : MonoBehaviour {

    private bool hasActed = false;
    private bool isActing = false;
    [Range(0,1)]
    public float difficulty = 0;
    public float minAnimSpeed = 1;
    public float maxAnimSpeed = 3;
    public float maxTime = 10f;
    public float minTime = 5f;
    public Animator anchorAnim;
    public Animator handAnim;
    public Animator headAnim;
    public Animator bodyAnim;
    public float anchorToHeadMaxDelta = 1.5f;
    public float headYOffset = -0.5f;
    MiniGame miniGame;

	// Use this for initialization
	void Awake ()
    {
        miniGame = GetComponent<MiniGame>();
	}

    void Start()
    {
        OneButtonInputMgr.Instance.OnButtonDown += OnButtonDown;
    }
	
    void OnEnable()
    {
        SetupMiniGame();
    }

    void OnDestroy()
    {
        if (OneButtonInputMgr.Instance != null)
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
        // set anim parameters
        int curr_stage = (int)(MiniGameMgr.Instance.GetLifeStage());
        int curr_iter = MiniGameMgr.Instance.GetRepeatTime();
        Debug.Log("stage | iter = " + curr_stage.ToString() + " | " + curr_iter.ToString());
        bodyAnim.SetInteger("stage", curr_stage);
        bodyAnim.SetInteger("iter", curr_iter);

        // set anim speeds (above and beyond mgr-driven overall speed changes)
        float speed = Mathf.Lerp(minAnimSpeed, maxAnimSpeed, difficulty);
        handAnim.speed = speed;
        bodyAnim.speed = speed;
        headAnim.speed = speed;
        anchorAnim.speed = speed;

        float duration = Mathf.Lerp(maxTime, minTime, difficulty);
        miniGame.SetTime(duration);
    }

    void ResolveAction()
    {
        //Debug.Log("WOO LET'S RESOLVE THIS ACTION");
        float anchor_y = anchorAnim.transform.position.y;
        float head_y = headAnim.transform.position.y + headYOffset;
        float y_delta = anchor_y - head_y;

        Debug.Log("Delta is " + y_delta.ToString());

        if (y_delta < 0 - anchorToHeadMaxDelta)
        {
            handAnim.SetTrigger("result_too_low");
            anchorAnim.Stop();
            headAnim.SetTrigger("result_fail_01");
            bodyAnim.SetTrigger("result_fail_01");
            miniGame.ReportLose();
        }
        else if (y_delta > anchorToHeadMaxDelta)
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
