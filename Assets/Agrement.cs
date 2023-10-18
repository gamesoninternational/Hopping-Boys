using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agrement : MonoBehaviour {
	
	string AGREE_KEY="agreement5";

	// Use this for initialization
	void Start () {
		int agreement = PlayerPrefs.GetInt (AGREE_KEY, 0);
		print ("____Agreement:"+agreement);
		gameObject.SetActive (agreement == 0);

		
	}
	
	public void OnOkButton(){
		PlayerPrefs.SetInt(AGREE_KEY,1);
		PlayerPrefs.Save ();
		gameObject.SetActive (false);
	}
}
