using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderGradientController : MonoBehaviour {

   Image fill;
   public Gradient gradient;

   void Init()
   {
      fill = GetComponent<Slider>().fillRect.GetComponent<Image>();
   }

   public void UpdateColor(float percent)
   {
      if (fill == null)
         Init();
      fill.color = gradient.Evaluate(percent);
   }
}
