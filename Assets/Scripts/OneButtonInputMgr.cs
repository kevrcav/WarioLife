﻿using UnityEngine;
using System.Collections;

public class OneButtonInputMgr : MonoBehaviour {

   public static OneButtonInputMgr Instance;

   public delegate void ButtonHandler();
   public ButtonHandler OnButtonDown;
   public ButtonHandler OnButtonUp;

   float timePressed;
   bool buttonDown;

   void Awake ()
   {
      Instance = this;
   }
	
	// Update is called once per frame
	void Update () {

      if (Input.GetButtonDown("OneButton"))
      {
         buttonDown = true;
         if (OnButtonDown != null)
            OnButtonDown();
      }

      if (Input.GetButtonUp("OneButton"))
      {
         buttonDown = false;
         timePressed = 0;
         if (OnButtonUp != null)
            OnButtonUp();
      }

      if (buttonDown)
         timePressed += Time.deltaTime;

   }

   public float GetButtonPressedTime()
   {
      return timePressed;
   }

   public bool GetButtonPressed()
   {
      return buttonDown;
   }
}