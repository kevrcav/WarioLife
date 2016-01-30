using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingMgr : MonoBehaviour {

   public static LoadingMgr Instance;

   public delegate void GameLoadedHandler(MiniGame game);
   public GameLoadedHandler OnSceneLoaded;

   public delegate void GameUnloadedHandler(string game);
   public GameUnloadedHandler OnSceneUnloaded;
   
   void Awake()
   {
      Instance = this;
   }

   public void LoadScene(string scene)
   {
      StartCoroutine(LoadSceneCoroutine(scene));
   }

   IEnumerator LoadSceneCoroutine(string scene)
   {
      yield return SceneManager.LoadSceneAsync(scene);
   }

   public void MiniGameLoaded(MiniGame minigame)
   {
      OnSceneLoaded(minigame);
      minigame.gameObject.SetActive(false);
   }

   public void UnloadScene(MiniGame miniGame)
   {
      string miniGameName = miniGame.name;
      SceneManager.UnloadScene(miniGameName);
      if (OnSceneUnloaded != null)
         OnSceneUnloaded(miniGameName);
   }
}
