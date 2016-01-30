using UnityEngine;
using System.Collections;

public class FriendMinigame : MonoBehaviour {

    public bool isLooking;
    public float minWaitTime;
    public float maxWaitTime;

    // Use this for initialization
    void Start () {
        StartCoroutine(LookCycle());
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.anyKey)
        {
            if (!isLooking)
            {
                MiniGame.Instance.ReportWin();
            }
            else
            {
                MiniGame.Instance.ReportLose();
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
