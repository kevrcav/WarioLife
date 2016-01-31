using UnityEngine;
using System.Collections;

public class ButtonTest : MonoBehaviour {

   SpriteRenderer sRenderer;

	void Start () {
      sRenderer = GetComponent<SpriteRenderer>();
      OneButtonInputMgr.Instance.OnButtonDown += ButtonDown;
      OneButtonInputMgr.Instance.OnButtonUp += ButtonUp;
   }
	
	// Update is called once per frame
	void Update () {
	
	}

   void ButtonDown()
   {
      sRenderer.color = Color.red;
   }

   void ButtonUp()
   {
      sRenderer.color = Color.white;
   }
}
