using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public GameObject _tutorial;
    public GameObject _worldSelector;

    public static bool _sfxState = true;
    public static bool _musicState = true;

    public AudioSource _backgroundMusic;

    public UnityEngine.UI.Toggle _musicToggle;
    public UnityEngine.UI.Toggle _sfxToggle;
    public GameObject _CharacterSelection;
    
	// Use this for initialization
	void Start () {
        _sfxToggle.isOn = _sfxState;
        _musicToggle.isOn = _musicState;
	}

    void OnEnable()
    {
        BlackOverlay.Instance.FadeOut();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnRateus()
    {
        Application.OpenURL("market://details?id=" + Application.identifier);
    }

    public void OnMoreApps()
    {
        Application.OpenURL("market://search?q=pub Facebook");
    }
    
    public void OnTutorial()
    {
        BlackOverlay.Instance.FadeIn();
        Invoke("TutorialAfterDelay", 0.5f);
    }

    private void TutorialAfterDelay()
    {
        gameObject.SetActive(false);
        _tutorial.SetActive(true);
    }

    public void OnPlay()
    {
        BlackOverlay.Instance.FadeIn();
        Invoke("PlayAfterDelay", 0.5f);
    }

    private void PlayAfterDelay()
    {
        gameObject.SetActive(false);
        _worldSelector.SetActive(true);
    }

    public void SfxToggled(bool value)
    {
        _sfxState = value;
        Camera.main.GetComponent<AudioSource>().volume = (value ? 1 : 0);
    }

    public void MusicToggled(bool value)
    {
        _musicState = value;
        _backgroundMusic.volume = (value ? 1 : 0);
    }
    public void OnCharacterSelection ()
    {
        BlackOverlay.Instance.FadeIn();
        Invoke("SelectionAfterDelay", 0.5f);
        Invoke("teteng",1f);

    }

    private void teteng(){
        BlackOverlay.Instance.FadeOut();
    }

    private void SelectionAfterDelay()
    {
        gameObject.SetActive(false);
        _CharacterSelection.SetActive(true);
    }
    public GameObject Agreements;
    public void btn_agree(string kemana){
       Application.OpenURL(kemana);
       
    }
}
