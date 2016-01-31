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

   SliderGradientController timerGradient;
   SliderGradientController happinessGradient;

   void Awake()
   {
      Instance = this;
      timerGradient = timer.GetComponent<SliderGradientController>();
      happinessGradient = happiness.GetComponent<SliderGradientController>();
   }

   public void StartBridgeSequence(string gameMessage)
   {
      happiness.value = MiniGameMgr.Instance.GetHappiness();
      happiness.gameObject.SetActive(true);
      happinessGradient.UpdateColor(happiness.value);
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
       timerGradient.UpdateColor(percentTime);
   }

   public void SetTimerOn(bool on)
   {
       timer.gameObject.SetActive(on);
   }
}
