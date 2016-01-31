using UnityEngine;
using System.Collections;

public class TombstoneText : MonoBehaviour {

   TextMesh text;

	// Use this for initialization
	void Awake () {
      text = GetComponent<TextMesh>();
	}
	
	public void SetTombText(string tombText)
   {
      text.text = tombText;
   }
}
