using UnityEngine;
using System.Collections;

public class MiniGame : MonoBehaviour {

   [Range(5, 10)]
   public int gameTime = 5;

   public bool winOnTimeOut;

   float currentTime;
   bool playing = true;
   bool won = false;

   public string instruction;
   public string[] completeLines;
   public string[] failLines;

   public static MiniGame Instance;

   void Awake()
   {
      Instance = this;
   }

	public void ReportWin()
   {
      if (!playing) return;
      MiniGameMgr.Instance.Report(true);
      playing = false;
      won = true;
   }

   public void ReportLose()
   {
      if (!playing) return;
      MiniGameMgr.Instance.Report(false);
      playing = false;
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
         HUDMgr.Instance.SetTimeRemaining(currentTime);
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
         return failLines[Random.Range(0, failLines.Length)];
      else if (completeLines.Length > 0)
         return completeLines[Random.Range(0, completeLines.Length)];
      else
         return "";
   }

   public string GetInstruction()
   {
      return instruction;
   }
}
