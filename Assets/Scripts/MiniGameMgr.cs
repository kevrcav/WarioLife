using UnityEngine;
using System.Collections;

[System.Serializable]
public struct LifeStage
{
   public Stage stage;
   public int numRepeats;
   public string[] minigames;
} 

public enum Stage
{
   kBaby,
   kYouth,
   kHipster,
   kGrownUp,
   kDecrepit
}

public class MiniGameMgr : MonoBehaviour {

   public static MiniGameMgr Instance;

   public LoadingDoors loadingDoors;
   public LifeStage[] miniGames;
   Stage currentLifeStage = Stage.kBaby;
   int currentMinigameIndex;
   int currentRepeats;
   public string[] completeLines;
   public string[] failLines;
   bool nextGameLoaded;
   bool lastGameUnloaded = true;
   bool betweenSequenceFinished;

   bool oneGameMode;

   float currentSpeed = 1;
   float happiness = 1;
   public float endingSpeed = 3;
   public float increment = .2f;
   public int speedUpFrequency = 5;

   string currentMinigameName;
   MiniGame currentMinigame;

   bool lastGameWon = true;

   void Awake()
   {
      Instance = this;
   }

   void Start()
   {
      string minigameToTest = PlayerPrefs.GetString("minigame_to_test");
      if (minigameToTest != "")
      {
         PlayerPrefs.SetString("minigame_to_test", "");
         currentMinigameName = minigameToTest;
         oneGameMode = true;
      }
      LoadingMgr.Instance.OnSceneLoaded += Instance_OnSceneLoaded;
      LoadingMgr.Instance.OnSceneUnloaded += Instance_OnSceneUnloaded;
      loadingDoors.OnDoorsOpened += Instance_OnDoorsOpened;
      loadingDoors.OnDoorsClosed += Instance_OnDoorsClosed;

      currentMinigameIndex = -1;
      ChooseMinigame();
      lastGameUnloaded = true;
      LoadingMgr.Instance.LoadScene(currentMinigameName);
      StartCoroutine(MiniGameBridge());
      HUDMgr.Instance.message.text = "Get Ready!";
   }

   void Instance_OnSceneLoaded(MiniGame minigame)
   {
      nextGameLoaded = true;
      currentMinigame = minigame;
      StartMinigameIfReady();
   }

    void Instance_OnSceneUnloaded(string unloadedMinigame)
   {
      lastGameUnloaded = true;
      StartMinigameIfReady();
   }
   
   void Instance_OnDoorsOpened()
   {
       HUDMgr.Instance.SetTimerOn(true);
   }

   void Instance_OnDoorsClosed()
   {
      lastGameUnloaded = false;
      nextGameLoaded = false;
      betweenSequenceFinished = false;
      StartCoroutine(MiniGameBridge());
      if (currentMinigame != null)
         LoadingMgr.Instance.UnloadScene(currentMinigame);
      else
         lastGameUnloaded = true;
      ChooseMinigame();
      LoadingMgr.Instance.LoadScene(currentMinigameName);
   }

   IEnumerator MiniGameBridge()
   {
      string completeLine = ""; 
      if (currentMinigame != null)
         completeLine = currentMinigame.GetCompleteLine();
      if (completeLine == "")
         completeLine = GetDefaultCompleteLine();
      HUDMgr.Instance.StartBridgeSequence(completeLine);
      yield return new WaitForSeconds(3);
      HUDMgr.Instance.EndBridgeSequence(currentMinigame.GetInstruction());
      betweenSequenceFinished = true;
      StartMinigameIfReady();
   }

   string GetDefaultCompleteLine()
   {
      if (lastGameWon)
         return completeLines[Random.Range(0, completeLines.Length)];
      else
         return failLines[Random.Range(0, failLines.Length)];
   }

   void StartMinigameIfReady()
   {
      if (lastGameUnloaded && nextGameLoaded && betweenSequenceFinished)
      {
         loadingDoors.OpenDoors();
         currentMinigame.StartGame();
      }
   }

   void ChooseMinigame()
   {
      if (oneGameMode)
      {
         return;
      }

      ++currentMinigameIndex;
      int numberInStage = miniGames[(int)currentLifeStage].minigames.Length;
      if (currentMinigameIndex < numberInStage)
      {
         currentMinigameName = miniGames[(int)currentLifeStage].minigames[currentMinigameIndex];
         return;
      }

      currentMinigameIndex = -1;
      if (currentRepeats < miniGames[(int)currentLifeStage].numRepeats)
         currentRepeats++;
      else
      {
         currentRepeats = 0;
         currentLifeStage = (Stage) (((int)currentLifeStage + 1) % miniGames.Length);
      }
      
      ChooseMinigame();
   }

   public void Report(bool won)
   {
      lastGameWon = won;
      loadingDoors.CloseDoors();
      HUDMgr.Instance.SetTimerOn(false);
      if (happiness <= 0)
         GameOver();
   }

   public Stage GetLifeStage()
   {
      return currentLifeStage;
   }

   public int GetRepeatTime()
   {
      return currentRepeats;
   }

   public float GetHappiness()
   {
      return happiness;
   }

   public void ChangeHappiness(float change)
   {
      happiness = Mathf.Clamp01(happiness + change);
   }

   void IncreaseSpeed()
   {
      currentSpeed = Mathf.Min(currentSpeed + increment, endingSpeed);
      Time.timeScale = currentSpeed;
   }

   void GameOver()
   {
       HUDMgr.Instance.message.text = "YOU LOSE! PRESS ENTER.";
       Time.timeScale = 0;
   }

   void OnGUI()
   {
       Restart();
   }

   void Restart()
   {
       if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Return))
       {
          Time.timeScale = 1;
          happiness = 1;
       }
   }
}
