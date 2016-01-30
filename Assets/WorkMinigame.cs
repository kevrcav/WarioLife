using UnityEngine;
using System.Collections;

public class WorkMinigame : MonoBehaviour {

    public bool resultsRunning;
    public bool swaying;
    public float pickTime;
    public float minSwayTime;
    public float maxSwayTime;
    public Animator chooser;
    private bool goLeft;

    // Use this for initialization
    void Start() {
        swaying = true;
        resultsRunning = false;
        StartCoroutine(Sway());
    }

    void Update()
    {
        if (Input.anyKey)
        {
            chooser.SetTrigger("Stop");
            resultsRunning = true;
            if (swaying)
            {
                MiniGame.Instance.ReportLose();
            }
            if (!swaying)
            {
                MiniGame.Instance.ReportWin();
            }
        }
    }

    IEnumerator Sway()
    {
        yield return new WaitForSeconds(Random.Range(minSwayTime, maxSwayTime));
        while (!resultsRunning) {
            swaying = !swaying;
            if (swaying)
            {
                chooser.SetTrigger("Sway");
                yield return new WaitForSeconds(Random.Range(minSwayTime, maxSwayTime));
            }   
            if (!swaying)
            {
                goLeft = (Random.value < 0.5);
                if (goLeft == true)
                {
                    chooser.SetTrigger("ChooseLeft");
                    yield return new WaitForSeconds(pickTime);
                }
                if (goLeft == false)
                {
                    chooser.SetTrigger("ChooseRight");
                    yield return new WaitForSeconds(pickTime);
                }

            }         
        }
    }
    
}
