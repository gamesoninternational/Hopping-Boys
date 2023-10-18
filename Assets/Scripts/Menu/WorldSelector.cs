using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSelector : MonoBehaviour {

    public GameObject _mainMenu;

    public Button[] _worldButtons;

    public LevelSelector _levelSelector;

	public bool _lockLevels = false;

	// Use this for initialization
	void Start () {
		
	}

    void OnEnable()
    {
		// Comment out if you want to delete all user data(like level unlocks, coins, etc...)
		// PlayerPrefs.DeleteAll ();
		// Once delete, don't forget to comment it back...

        BlackOverlay.Instance.FadeOut();
        // Process worlds unlock
        for(int i = 0; i < _worldButtons.Length; i++)
        {
			bool unlocked = (PlayerPrefs.GetInt(string.Format("World{0}Unlocked", i), 0) == 1 || i == 0 || !_lockLevels);
            _worldButtons[i].interactable = unlocked;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSelectWorld(int levelIndex)
    {
		if (_levelSelector.SomeLevelsUnlocked(levelIndex) || _lockLevels) // Display level selector
        {
            gameObject.SetActive(false);
			_levelSelector._lockLevels = _lockLevels;
            _levelSelector._levelIndex = levelIndex;
            _levelSelector.gameObject.SetActive(true);
        }
        else // Load level directly
        {
            GameManager._currentLevelIndex = 0;
            Application.LoadLevel(levelIndex + 1);
        }
    }

    public void OnBack()
    {
        BlackOverlay.Instance.FadeIn();
        Invoke("BackAfterDelay", 0.5f);
    }

    void BackAfterDelay()
    {
        gameObject.SetActive(false);
        _mainMenu.SetActive(true);
    }

}
