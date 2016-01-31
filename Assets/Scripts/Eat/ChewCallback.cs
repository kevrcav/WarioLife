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
        Stage curr_stage = MiniGameMgr.Instance.GetLifeStage();
        int curr_iter = MiniGameMgr.Instance.GetMinigameRepeats("eat_minigame");

        Debug.Log("Food is done!");
        if (curr_stage == Stage.kBaby && curr_iter == 2)
            cheerioParticles.Play();
        else if (curr_stage == Stage.kYouth && curr_iter == 2)
        {
            Debug.Log("Launching ice cream!");
            iceCreamParticles.Play();
        }
        else if (curr_stage == Stage.kHipster && curr_iter == 0)
        {
            Debug.Log("Launching puke!");
            beerParticles.Play();
        }
//        else if (curr_stage == Stage.kDecrepit && curr_iter == 2)
//        {
//            Debug.Log("Launching puke!");
//            beerParticles.Stop();
//            beerParticles.Play();
//        }


    }

    public void OnStartSound()
    {
        Stage curr_stage = MiniGameMgr.Instance.GetLifeStage();
        int curr_iter = MiniGameMgr.Instance.GetMinigameRepeats("eat_minigame");

        if (curr_stage == Stage.kBaby)
        {
            if (curr_iter == 2)
                AudioMgr.Instance.PlaySFX(SoundEffectType.kBabyCoo);
            else
                AudioMgr.Instance.PlaySFX(SoundEffectType.kBabyLaugh);
        }
    }

    public void OnChewStartSound()
    {
        Stage curr_stage = MiniGameMgr.Instance.GetLifeStage();
        int curr_iter = MiniGameMgr.Instance.GetMinigameRepeats("eat_minigame");

        // sound
        // pick sound
        if ((curr_stage == Stage.kBaby && curr_iter == 2)
            || (curr_stage == Stage.kHipster && curr_iter == 0))
        {
            AudioMgr.Instance.PlaySFX(SoundEffectType.kVomit);
        }
        else if ((curr_stage == Stage.kGrownUp && curr_iter == 1)
            || (curr_stage == Stage.kGrownUp && curr_iter == 2))
        {
            AudioMgr.Instance.PlaySFX(SoundEffectType.kDrinking);
        }
        else if ((curr_stage == Stage.kBaby && curr_iter == 1)
            || (curr_stage == Stage.kDecrepit && curr_iter == 2))
        {
            AudioMgr.Instance.PlaySFX(SoundEffectType.kChewingSoft);
        }
        else
            AudioMgr.Instance.PlaySFX(SoundEffectType.kChewingCrunch);
    }

    public void OnFailSound()
    {
        AudioMgr.Instance.PlaySFX(SoundEffectType.kSadEnd);
    }
}
