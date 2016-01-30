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
            playerAnimator.SetTrigger("Action");
            if (isLooking)
            {
                friendAnimator.SetTrigger("ReactGood");
                MiniGame.Instance.ReportWin(1.5f);
            }
            else
            {
                friendAnimator.SetTrigger("ReactBad");
                MiniGame.Instance.ReportLose(1.5f);
            }
        }
	}

    IEnumerator LookCycle()
    {
        isLooking = (Random.value < 0.5);
        while (true) {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            isLooking = !isLooking;
        }
    }

}
