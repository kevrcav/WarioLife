using UnityEngine;
using System.Collections;

public class ChewCallback : MonoBehaviour {

    public ParticleSystem cheerioParticles;
    public ParticleSystem hotdogParticles;
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
        if (mgmgr.GetLifeStage() == Stage.kBaby && mgmgr.GetRepeatTime() == 2)
            cheerioParticles.Play();
        else if (mgmgr.GetLifeStage() == Stage.kYouth && mgmgr.GetRepeatTime() == 2)
        {
            Debug.Log("Launching guitar!");
            hotdogParticles.Play();
        }
    }
}
