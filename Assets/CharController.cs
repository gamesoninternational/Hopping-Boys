using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {

	public GameObject adsConfrimPopup;

	public static CharController Instance;

	void Awake(){
		Instance = this;
	}
	void Start(){
		UpdateAdsPopup (false);
	}

	public void UpdateAdsPopup(bool isShow){
	
		adsConfrimPopup.SetActive (isShow);
		
	}

	public void tampil_iklan(){
		Char.Instance.ShowAdd(true);
	}
}
