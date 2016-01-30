using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MiniGame : MonoBehaviour {
   
   public float gameTime = 5;

   public float happinessReward;
   public float happinessPenalty;

   public bool winOnTimeOut;

   float currentTime;
   bool playing = true;
   bool won = false;
   bool useSpecificLine;
   int specificLine;

   public string instruction;
   public string[] completeLines;
   public string[] failLines;

   public static MiniGame Instance;

   void Awake()
   {
      Instance = this;
      if (LoadingMgr.Instance == null)
      {
         PlayerPrefs.SetString("minigame_to_test", name);
         SceneManager.LoadScene("main");
         return;
      }
      LoadingMgr.Instance.MiniGameLoaded(this);
   }

   public void SetHappinessReward(float reward)
   {
      happinessReward = reward;
   }

   public void SetHappinessPenalty(float penalty)
   {
      happinessPenalty = penalty;
   }

	public void ReportWin(float winDelay = 1)
   {
      if (!playing) return;
      MiniGameMgr.Instance.ChangeHappiness(happinessReward);
      StartCoroutine(ReportAfterDelay(winDelay, true));
      playing = false;
      won = true;
   }

   public void ReportLose(float loseDelay = 1)
   {
      if (!playing) return;
      MiniGameMgr.Instance.ChangeHappiness(happinessPenalty);
      StartCoroutine(ReportAfterDelay(loseDelay, false));
      playing = false;
   }

   public void SetEndLine(int index)
   {
      useSpecificLine = true;
      specificLine = index;
   }

   public void SetTime(int time)
   {
      gameTime = time;
      currentTime = time;
   }

   IEnumerator ReportAfterDelay(float winDelay, bool win)
   {
      yield return new WaitForSeconds(winDelay);
      MiniGameMgr.Instance.Report(win);
   }

   public void StartGame()
   {
      currentTime = gameTime;
      playing = true;
      gameObject.SetActive(true);
   }

   void Update()
   {
      if (playing)
      {
         currentTime -= Time.deltaTime;
         HUDMgr.Instance.SetTimeRemaining(currentTime / gameTime);
         if (currentTime <= 0)
         {
            if (winOnTimeOut)
               ReportWin();
            else
               ReportLose();
         }
      }
   }

   public string GetCompleteLine()
   {
      if (!won && failLines.Length > 0)
         return failLines[useSpecificLine ? specificLine : Random.Range(0, failLines.Length)];
      else if (completeLines.Length > 0)
         return completeLines[useSpecificLine ? specificLine : Random.Range(0, completeLines.Length)];
      else
         return "";
   }

   public string GetInstruction()
   {
      return instruction;
   }
}
