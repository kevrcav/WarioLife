using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDMgr : MonoBehaviour {

   public static HUDMgr Instance;
   
   public Text message;
   public Slider happiness;
   public Slider timer;
   public Text continueText;
   public Text youDiedText;
   public Text startText;

   static string livestext = "LIVES: ";
   static string scoretext = "SCORE: ";

   public float bridgeTime;

   SliderGradientController timerGradient;
   SliderGradientController happinessGradient;

   bool init;

   void Awake()
   {
      Instance = this;
      timerGradient = timer.GetComponent<SliderGradientController>();
      happinessGradient = happiness.GetComponent<SliderGradientController>();
   }

   public void Init()
   {
      if (init) return;
      init = true;
      happiness.gameObject.SetActive(true);
      timer.gameObject.SetActive(true);
      message.gameObject.SetActive(true);
   }

   public void StartBridgeSequence(string gameMessage)
   {
      happiness.value = MiniGameMgr.Instance.GetHappiness();
      happinessGradient.UpdateColor(happiness.value);
      message.text = gameMessage;
      message.gameObject.SetActive(true);
   }

   public void SetMessage(string gameMessage)
   {
      message.text = gameMessage;
   }

   public void EndBridgeSequence(string gameMessage)
   {
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

   public void ShowStart(bool b)
   {
      startText.gameObject.SetActive(b);
   }

   public void ShowContinue(bool b)
   {
      continueText.gameObject.SetActive(b);
   }

   public void DisplayYouDiedText()
   {
      youDiedText.gameObject.SetActive(true);
      happiness.gameObject.SetActive(false);
      timer.gameObject.SetActive(false);
   }
}
