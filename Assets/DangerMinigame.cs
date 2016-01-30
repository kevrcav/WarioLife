using UnityEngine;
using System.Collections;

public class DangerMinigame : MonoBehaviour {

    public Transform player;
    public Transform threat;
    public float speed;
    public bool startedMoving;
    public float farEnough;
    public float tooDamnFar;
    public bool resultsRunning;

	// Use this for initialization
	void Start () {
        startedMoving = false;
        resultsRunning = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!resultsRunning)
        {
	        if (Input.anyKey)
            {
                startedMoving = true;
                float step = speed * Time.deltaTime;
                player.position = Vector3.MoveTowards(player.position, threat.position, step);
            }
            if (!Input.anyKey && startedMoving && (player.position.x > farEnough) && (player.position.x < tooDamnFar))
            {
                resultsRunning = true;
                MiniGame.Instance.ReportWin(1.5f);
            }
            if (!Input.anyKey && startedMoving && (player.position.x < farEnough))
            {
                resultsRunning = true;
                MiniGame.Instance.ReportLose(1.5f);
            }
            if (player.position.x > tooDamnFar)
            {
                resultsRunning = true;
                MiniGame.Instance.ReportLose(1.5f);
            }
        }

    }
}
