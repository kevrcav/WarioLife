using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingMgr : MonoBehaviour {

   public static LoadingMgr Instance;

   public delegate void GameLoadedHandler(MiniGame game);
   public GameLoadedHandler OnSceneLoaded;

   public delegate void FinalSceneLoadedHandler(TombstoneText tombstone);
   public FinalSceneLoadedHandler OnFinalSceneLoaded;

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
      yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
   }

   public void MiniGameLoaded(MiniGame minigame)
   {
      OnSceneLoaded(minigame);
      minigame.gameObject.SetActive(false);
   }

   public void UnloadScene(MiniGame miniGame)
   {
      string miniGameName = miniGame.name;
      StartCoroutine(UnloadNextFrame(miniGameName));
      if (OnSceneUnloaded != null)
         OnSceneUnloaded(miniGameName);
   }

   IEnumerator UnloadNextFrame(string scene)
   {
      yield return new WaitForEndOfFrame();
      SceneManager.UnloadScene(scene);
   }

   public void LoadFinalScene()
   {
      StartCoroutine(LoadFinalSceneCoroutine());
   }

   IEnumerator LoadFinalSceneCoroutine()
   {
      yield return SceneManager.LoadSceneAsync("dead_minigame", LoadSceneMode.Additive);
      if (OnFinalSceneLoaded != null)
         OnFinalSceneLoaded(FindObjectOfType<TombstoneText>());
   }

   public static void Reload()
   {
      SceneManager.LoadScene("main");
   }
}
