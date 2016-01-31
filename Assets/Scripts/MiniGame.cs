using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MiniGame : MonoBehaviour {
   
   public float gameTime = 5;

   public float happinessReward;
   public float happinessPenalty;

   public bool winOnTimeOut;

   float currentTime;
   bool playing = false;
   bool won = false;
   bool useSpecificLine;
   int specificLine;

   public string instruction;
   public string[] completeLines;
   public string[] failLines;

   public string tombLine;
   public string livedLine;

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
      MiniGameMgr.Instance.ChangeHappiness(-happinessPenalty);
      Debug.Log(MiniGameMgr.Instance.GetHappiness());
      StartCoroutine(ReportAfterDelay(loseDelay, false));
      playing = false;
   }

   public void SetEndLine(int index)
   {
      useSpecificLine = true;
      specificLine = index;
   }

   public void SetTime(float time)
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
      gameObject.SetActive(true);
   }

   public void StartTimer()
   {
      playing = true;
   }

   void Update()
   {
      HUDMgr.Instance.SetTimeRemaining(currentTime / gameTime);
      if (playing)
      {
         currentTime -= Time.deltaTime;
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

   public string GetTombLine()
   {
      return tombLine;
   }

   public void SetTombLine(string line)
   {
      tombLine = line;
   }

   public string GetLivedLine()
   {
      return livedLine;
   }

   public void SetLivedLine(string line)
   {
      livedLine = line;
   }
}
