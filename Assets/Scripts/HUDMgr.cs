using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDMgr : MonoBehaviour {

   public static HUDMgr Instance;

   public Text lives;
   public Text score;
   public Text message;
   public Text timer;

   static string livestext = "LIVES: ";
   static string scoretext = "SCORE: ";

   public float bridgeTime;

   void Awake()
   {
      Instance = this;
   }

   public void StartBridgeSequence(string gameMessage)
   {
      lives.text = livestext + MiniGameMgr.Instance.lives.ToString();
      lives.gameObject.SetActive(true);
      score.text = scoretext + MiniGameMgr.Instance.score.ToString();
      score.gameObject.SetActive(true);
      message.text = gameMessage;
      message.gameObject.SetActive(true);
   }

   public void EndBridgeSequence(string gameMessage)
   {
      lives.gameObject.SetActive(false);
      score.gameObject.SetActive(false);
      message.text = gameMessage;
      message.gameObject.SetActive(true);
   }

   public void SetTimeRemaining(float timeRemaning)
   {
       timer.text = Mathf.CeilToInt(timeRemaning).ToString();
   }

   public void SetTimerOn(bool on)
   {
       timer.gameObject.SetActive(on);
   }
}
