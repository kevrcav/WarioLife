using UnityEngine;
using System.Collections;

public class MiniGameMgr : MonoBehaviour {

   public static MiniGameMgr Instance;

   public LoadingDoors loadingDoors;
   public int score;
   public int lives;
   public string[] miniGames;
   public string[] completeLines;
   public string[] failLines;
   bool nextGameLoaded;
   bool lastGameUnloaded;
   bool betweenSequenceFinished;

   float currentSpeed = 1;
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
      LoadingMgr.Instance.OnSceneLoaded += Instance_OnSceneLoaded;
      LoadingMgr.Instance.OnSceneUnloaded += Instance_OnSceneUnloaded;
      loadingDoors.OnDoorsOpened += Instance_OnDoorsOpened;
      loadingDoors.OnDoorsClosed += Instance_OnDoorsClosed;

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
      LoadingMgr.Instance.UnloadScene(currentMinigame);
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
      string lastMiniGame = currentMinigameName;
      currentMinigameName = miniGames[Random.Range(0, miniGames.Length)];
      while (miniGames.Length > 1 && lastMiniGame == currentMinigameName)
         currentMinigameName = miniGames[Random.Range(0, miniGames.Length)];
   }

   public void Report(bool won)
   {
      lastGameWon = won;
      loadingDoors.CloseDoors();
      HUDMgr.Instance.SetTimerOn(false);
      if (won)
      {
          if (++score % speedUpFrequency == 0)
              IncreaseSpeed();
      }
      else
      {
          if (--lives <= 0)
              GameOver();
      }
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
          lives = 4;
          score = 0;
       }
   }
}
