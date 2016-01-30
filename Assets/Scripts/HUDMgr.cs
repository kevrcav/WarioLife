using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDMgr : MonoBehaviour {

   public static HUDMgr Instance;
   
   public Text message;
   public Slider happiness;
   public Slider timer;

   static string livestext = "LIVES: ";
   static string scoretext = "SCORE: ";

   public float bridgeTime;

   void Awake()
   {
      Instance = this;
   }

   public void StartBridgeSequence(string gameMessage)
   {
      happiness.value = MiniGameMgr.Instance.GetHappiness();
      happiness.gameObject.SetActive(true);
      message.text = gameMessage;
      message.gameObject.SetActive(true);
   }

   public void EndBridgeSequence(string gameMessage)
   {
      happiness.gameObject.SetActive(false);
      message.text = gameMessage;
      message.gameObject.SetActive(true);
   }

   public void SetTimeRemaining(float percentTime)
   {
       timer.value = percentTime;
   }

   public void SetTimerOn(bool on)
   {
       timer.gameObject.SetActive(on);
   }
}
