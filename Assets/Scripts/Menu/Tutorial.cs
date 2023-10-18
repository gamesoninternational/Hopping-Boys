using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public GameObject _mainMenu;

	// Use this for initialization
	void Start () {
		
	}

    void OnEnable()
    {
        BlackOverlay.Instance.FadeOut();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnBack()
    {
        BlackOverlay.Instance.FadeIn();
        Invoke("BackAfterDelay", 0.5f);
    }

    private void BackAfterDelay()
    {
        gameObject.SetActive(false);
        _mainMenu.SetActive(true);
    }

}
