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

    void Start () {
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
            if (isLooking)
            {
                StartCoroutine(GoodEnding());
            }
            else
            {
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
