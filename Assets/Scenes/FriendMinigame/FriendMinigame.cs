using UnityEngine;
using System.Collections;

public class FriendMinigame : MonoBehaviour {

    public bool isLooking;
    public float minWaitTime;
    public float maxWaitTime;
    public GameObject friendhead;
    public Animator playerAnimator;
    public Animator friendAnimator;
    private bool resultsRunning;
    public float endDelay;
    public Sprite[] spritelist;
    public SpriteRenderer gift;

    void Start () {

        int curr_stage = (int)(MiniGameMgr.Instance.GetLifeStage());
        int curr_iter = MiniGameMgr.Instance.GetMinigameRepeats("friend_minigame");
        playerAnimator.SetInteger("stage", curr_stage);
        playerAnimator.SetInteger("iter", curr_iter);
        friendAnimator.SetInteger("stage", curr_stage);
        friendAnimator.SetInteger("iter", curr_iter);
        gift.sprite = spritelist[(curr_stage * 3) + curr_iter];

        resultsRunning = false;
        StartCoroutine(LookCycle());
    }
	
	void Update () {
        if (!resultsRunning)
        {
            if (isLooking)
            {
                friendhead.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
            if (!isLooking)
            {
                friendhead.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        //input
        if (Input.anyKey)
        {
            resultsRunning = true;
            int curr_stage = (int)(MiniGameMgr.Instance.GetLifeStage());
            if (isLooking)
            {
                if (curr_stage > 3)
                {
                    MiniGame.Instance.SetHappinessPenalty(0.1f);
                    StartCoroutine(BadEnding());
                }
                else
                {
                    StartCoroutine(GoodEnding());
                }
            }
            else
            {
                if (curr_stage > 3)
                {
                    MiniGame.Instance.SetHappinessPenalty(0.1f);
                }
                StartCoroutine(BadEnding());
            }
        }
	}

    IEnumerator LookCycle()
    {
        isLooking = false;
        while (true) {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            isLooking = !isLooking;
        }
    }

    IEnumerator GoodEnding()
    {
        playerAnimator.SetTrigger("ActionGood");
        yield return new WaitForSeconds(endDelay);
        friendAnimator.SetTrigger("ReactGood");
        MiniGame.Instance.ReportWin();
    }

    IEnumerator BadEnding()
    {
        playerAnimator.SetTrigger("ActionBad");
        yield return new WaitForSeconds(endDelay);
        friendAnimator.SetTrigger("ReactBad");
        MiniGame.Instance.ReportLose();
    }
}
