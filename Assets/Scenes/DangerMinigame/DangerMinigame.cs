using UnityEngine;
using System.Collections;

public class DangerMinigame : MonoBehaviour {

    public Transform player;
    public Transform threat;
    public float speed;
    public bool farEnough;
    public bool tooDamnFar;
    public bool resultsRunning;
    public Animator animator;
    public SpriteRenderer backdrop;


    // Use this for initialization
    void Start () {
        int curr_stage = (int)(MiniGameMgr.Instance.GetLifeStage());
        animator.SetInteger("stage", curr_stage);
        animator.SetTrigger("go");
        resultsRunning = false;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKey && !resultsRunning && farEnough && !tooDamnFar)
        {
            resultsRunning = true;
            backdrop.color = Color.green;
            MiniGame.Instance.ReportWin(1.5f);
        }

        if (Input.anyKey && !resultsRunning && !farEnough)
        {
            resultsRunning = true;
            backdrop.color = Color.red;
            MiniGame.Instance.ReportLose(1.5f);
        }

        if (tooDamnFar && !resultsRunning)
        {
            resultsRunning = true;
            MiniGame.Instance.SetHappinessPenalty(.6f);
            MiniGame.Instance.ReportLose(1.5f);
        }
        if (resultsRunning)
        {
            animator.Stop();
        }
        
    }
}
