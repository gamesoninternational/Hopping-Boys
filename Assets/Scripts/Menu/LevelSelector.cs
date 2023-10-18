using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

    public int _levelIndex;

    private int _maxLevelCount = 20;

    public Button _prevBtn;
    public Button _nextBtn;
    public Text _levelTitle;

    public GameObject _parent;

    private int _currentLevelIndex = 0;
    private int _maxUnlockedLevel = 0;

	[HideInInspector]
	public bool _lockLevels = false;
    
    // Use this for initialization
	void Start () {
		
	}

    void OnEnable()
    {
        BlackOverlay.Instance.FadeOut();
        // Find last unlocked level
        _currentLevelIndex = _maxUnlockedLevel = 0;
        for(int i = _maxLevelCount - 1; i > 0; i--)
        {
			if(!_lockLevels || PlayerPrefs.GetInt(string.Format("World{0}Level{1}Unlocked", _levelIndex, i), 0) == 1)
            {
                _maxUnlockedLevel = _currentLevelIndex = i;
                break;
            }
        }
        UpdateSelectionBtns();
    }
	
    private void UpdateSelectionBtns()
    {
        _levelTitle.text = string.Format("{0:00}", (_currentLevelIndex + 1));
        _nextBtn.interactable = (_currentLevelIndex < _maxUnlockedLevel);
        _prevBtn.interactable = (_currentLevelIndex > 0);
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void OnStartPressed()
    {
        BlackOverlay.Instance.FadeIn();
        Invoke("StartAfterDelay", 0.5f);
    }

    private void StartAfterDelay()
    {
        GameManager._currentLevelIndex = _currentLevelIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(_levelIndex + 1);
    }

    public bool SomeLevelsUnlocked(int index)
    {
        for (int i = _maxLevelCount - 1; i > 0; i--)
        {
            if (PlayerPrefs.GetInt(string.Format("World{0}Level{1}Unlocked", index, i), 0) == 1)
                return true;
        }

        return false;
    }

    public void OnPrevPressed()
    {
        if (_currentLevelIndex > 0)
            _currentLevelIndex--;
        UpdateSelectionBtns();
    }

    public void OnNextPressed()
    {
        if (_currentLevelIndex < _maxUnlockedLevel)
            _currentLevelIndex++;
        UpdateSelectionBtns();
    }

    public void OnBackPressed()
    {
        BlackOverlay.Instance.FadeIn();
        Invoke("BackAfterDelay", 0.5f);
    }

    public void BackAfterDelay()
    {
        gameObject.SetActive(false);
        _parent.SetActive(true);
    }

}
