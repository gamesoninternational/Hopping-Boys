using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char : MonoBehaviour {

	public int id;
	public bool isLocked;
	public GameObject lockImage;

	public static Char Instance;

	void OnEnable()
	{
		Global.currentCharId = id;
		print ("selected id : "+Global.currentCharId);
	}

	void Awake(){
		Instance = this;
	}
	void Start(){
		UpdateUI ();
		
	}
	public void UpdateUI(){
		int locked = PlayerPrefs.GetInt (Global.UNLOCK_CHAR+id,0);
		if (locked==1) {
			isLocked = false;
		} else {
			isLocked = true;
		}
		lockImage.SetActive (isLocked);
	}
	public void OnUnlockChar(){
		print ("Call Unlock char");
		CharController.Instance.UpdateAdsPopup(true);


	}

	public void ShowAdd(bool isShow){
		if (isShow) {
			Global.currentChar = this;
			AdManager.Instance.ShowVideoReward ();
		} 
		CharController.Instance.UpdateAdsPopup(false);
	}

}
