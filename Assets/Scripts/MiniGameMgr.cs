using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[System.Serializable]
public struct LifeStage
{
   public Stage stage;
   public float speedStart;
   public float speedEnd;
   public string[] minigames;
   public int numIncrements;
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
   public string currentConfigName;
   public LifeStage[] miniGames;
   Stage currentLifeStage = Stage.kBaby;
   int currentMinigameIndex;
   public string[] completeLines;
   public string[] failLines;
   bool nextGameLoaded;
   bool lastGameUnloaded = true;
   bool betweenSequenceFinished;

   public TextAsset configData;
   Dictionary<string, LifeStage[]> minigameSets;
   Dictionary<string, int> currentMinigameIterations;
   int currentIncrements;
   bool newStage = true;

   bool oneGameMode;

   float currentSpeed = 1;
   float happiness = 1;
   public float endingSpeed = 3;
   public float increment = .2f;
   public int speedUpFrequency = 5;

   string currentMinigameName;
   MiniGame currentMinigame;

   bool lastGameWon = true;

   public bool cantLose;

   void Awake()
   {
      Instance = this;
   }

   void Start()
   {
      LoadMinigameConfig();

      miniGames = minigameSets[currentConfigName];

      currentMinigameIterations = new Dictionary<string, int>();

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

   void LoadMinigameConfig()
   {
      XmlDocument configs = new XmlDocument();
      configs.LoadXml(configData.text);

      minigameSets = new Dictionary<string, LifeStage[]>();

      foreach (XmlNode config in configs.SelectSingleNode("Minigames").ChildNodes)
      {
         List<LifeStage> stages = new List<LifeStage>();
         foreach (XmlNode stageNode in config.ChildNodes)
         {
            int numIncrements = 0;
            Stage lifeStage = GetLifeStageFromNode(stageNode);
            XmlNode speedNode = stageNode.SelectSingleNode("Speeds");
            float startSpeed = float.Parse(speedNode.SelectSingleNode("Start").InnerText);
            float endSpeed = float.Parse(speedNode.SelectSingleNode("End").InnerText);
            List<string> minigameNames = new List<string>();
            foreach (XmlNode minigameNode in stageNode.SelectSingleNode("Minigames"))
            {
               string miniName = minigameNode.Name;
               if (miniName == "increment")
                  ++numIncrements;
               minigameNames.Add(miniName);
            }
            LifeStage newLifeStage;
            newLifeStage.minigames = minigameNames.ToArray();
            newLifeStage.speedStart = startSpeed;
            newLifeStage.speedEnd = endSpeed;
            newLifeStage.stage = lifeStage;
            newLifeStage.numIncrements = numIncrements;
            stages.Add(newLifeStage);
         }
         minigameSets[config.Name] = stages.ToArray();
      }
   }

   Stage GetLifeStageFromNode(XmlNode stageNode)
   {
      switch (stageNode.Name)
      {
         case "Baby":
            return Stage.kBaby;
         case "Youth":
            return Stage.kYouth;
         case "Hipster":
            return Stage.kHipster;
         case "Grownup":
            return Stage.kGrownUp;
         case "Decrepit":
            return Stage.kDecrepit;
      }
      return Stage.kBaby;
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
      OneButtonInputMgr.Instance.SetIgnoreInput(false);
      currentMinigame.StartTimer();
   }

   void Instance_OnDoorsClosed()
   {
      lastGameUnloaded = false;
      nextGameLoaded = false;
      betweenSequenceFinished = false;
      if (newStage)
         StartCoroutine(BetweenStageBridge());
      else
         StartCoroutine(MiniGameBridge());
      if (currentMinigame != null)
         LoadingMgr.Instance.UnloadScene(currentMinigame);
      else
         lastGameUnloaded = true;
      LoadingMgr.Instance.LoadScene(currentMinigameName);
   }

   IEnumerator BetweenStageBridge()
   {
      string completeLine = "YOU LIVED: ";
      HUDMgr.Instance.StartBridgeSequence(completeLine);
      yield return new WaitForSeconds(2);
      HUDMgr.Instance.ShowContinue(true);
      while (!OneButtonInputMgr.Instance.GetButtonPressed(true))
      {
         yield return new WaitForEndOfFrame();
      }
      HUDMgr.Instance.ShowContinue(false);
      HUDMgr.Instance.EndBridgeSequence(currentMinigame.GetInstruction());
      betweenSequenceFinished = true;
      StartMinigameIfReady();
   }

   IEnumerator MiniGameBridge()
   {
      string completeLine = ""; 
      if (currentMinigame != null)
         completeLine = currentMinigame.GetCompleteLine();
      if (completeLine == "")
         completeLine = GetDefaultCompleteLine();
      HUDMgr.Instance.StartBridgeSequence(completeLine);
      yield return new WaitForSeconds(1);
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
         loadingDoors.OpenDoors(newStage);
         newStage = false;
         currentMinigame.StartGame();
         OneButtonInputMgr.Instance.SetIgnoreInput(true);
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
         if (currentMinigameName == "increment")
         {
            Increment();
            ChooseMinigame();
            return;
         }
         if (!currentMinigameIterations.ContainsKey(currentMinigameName))
            currentMinigameIterations[currentMinigameName] = -1;
         currentMinigameIterations[currentMinigameName]++;
         return;
      }

      currentMinigameIndex = -1;
      currentLifeStage = (Stage)(((int)currentLifeStage + 1) % miniGames.Length);
      NewStage();
      
      ChooseMinigame();
   }

   void Increment()
   {
      ++currentIncrements;
      if (miniGames[(int)currentLifeStage].numIncrements == 0) return;
      SetSpeed(Mathf.Lerp(miniGames[(int)currentLifeStage].speedStart,
                          miniGames[(int)currentLifeStage].speedEnd,
                          (float)currentIncrements / (float)miniGames[(int)currentLifeStage].numIncrements));
   }

   void NewStage()
   {
      newStage = true;
      currentSpeed = miniGames[(int)currentLifeStage].speedStart;
      currentIncrements = 0;
      currentMinigameIterations.Clear();
      Time.timeScale = 1;
   }

   public void Report(bool won)
   {
      lastGameWon = won;
      ChooseMinigame();
      loadingDoors.CloseDoors(newStage);
      if (!cantLose && happiness <= 0)
         GameOver();
   }

   public Stage GetLifeStage()
   {
      return currentLifeStage;
   }

   public int GetRepeatTime()
   {
      return 0;
   }

   public int GetMinigameRepeats(string minigame)
   {
      int numberMinigameReplays = 0;

      currentMinigameIterations.TryGetValue(minigame, out numberMinigameReplays);

      return numberMinigameReplays;
   }

   public float GetHappiness()
   {
      return happiness;
   }

   public void ChangeHappiness(float change)
   {
      happiness = Mathf.Clamp01(happiness + change);
   }

   void SetSpeed(float speed)
   {
      currentSpeed = speed;
      Time.timeScale = speed;
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
