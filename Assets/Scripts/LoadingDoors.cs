﻿using UnityEngine;
using System.Collections;

public class LoadingDoors : MonoBehaviour {

   Animator anim;

   public delegate void LoadingDoorStateChangedHandler();
   public LoadingDoorStateChangedHandler OnDoorsOpened;
   public LoadingDoorStateChangedHandler OnDoorsClosed;

	// Use this for initialization
	void Start () {
      anim = GetComponent<Animator>();
      anim.Play("Open", 0, 1);
	}
	
	public void OpenDoors (bool slow = false) 
   {
       anim.Play(slow ? "SlowOpen" : "Open");
	}
   public void CloseDoors(bool slow = false)
   {
      anim.Play(slow ? "SlowClose" : "Close");
   }

   public void DoorsOpened()
   {
      if (OnDoorsOpened != null)
         OnDoorsOpened();
   }

   public void DoorsClosed()
   {
      if (OnDoorsClosed != null)
         OnDoorsClosed();
   }
}
