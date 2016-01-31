using UnityEngine;
using System.Collections;

public class ChewCallback : MonoBehaviour {

    public ParticleSystem cheerioParticles;
    public ParticleSystem iceCreamParticles;
    public ParticleSystem beerParticles;
    private ParticleSystem.EmissionModule emissionMod;
    private MiniGameMgr mgmgr;

	// Use this for initialization
	void Start ()
    {
        mgmgr = MiniGameMgr.Instance;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnFoodEaten()
    {
        Debug.Log("Food is done!");
        if (mgmgr.GetLifeStage() == Stage.kBaby && MiniGameMgr.Instance.GetMinigameRepeats("eat_minigame") == 2)
            cheerioParticles.Play();
        else if (mgmgr.GetLifeStage() == Stage.kYouth && MiniGameMgr.Instance.GetMinigameRepeats("eat_minigame") == 2)
        {
            Debug.Log("Launching ice cream!");
            iceCreamParticles.Play();
        }
        else if (mgmgr.GetLifeStage() == Stage.kHipster && MiniGameMgr.Instance.GetMinigameRepeats("eat_minigame") == 0)
        {
            Debug.Log("Launching puke!");
            beerParticles.Play();
        }
        else if (mgmgr.GetLifeStage() == Stage.kDecrepit && MiniGameMgr.Instance.GetMinigameRepeats("eat_minigame") == 2)
        {
            Debug.Log("Launching puke!");
            beerParticles.Stop();
            beerParticles.Play();
        }
    }
}
