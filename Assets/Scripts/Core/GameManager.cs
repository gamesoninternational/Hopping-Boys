using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject[] _characterPrefab=new GameObject[3];
    public Transform _characterSpawnPoint;

    public CameraFollower _cameraFollower;

    [HideInInspector]
    public Character _currCharacter = null;

    public UnityEngine.UI.Text _coinsText;
    public UnityEngine.UI.Text _levelText;
    public UnityEngine.UI.Text _worldText;

    public GameObject[] _levelPrefabs;
    public static int _currentLevelIndex = 0;

    public float _initialSpeedSign = 1.0f;

    public static float TileUnitSize = 1.2f;

    private bool gameStarted = false;

    void Awake()
    {
		
        Time.timeScale = 1.0f;
        gameStarted = false;
        // Spawn level
        Tiled2Unity.TiledMap tmp = _levelPrefabs[_currentLevelIndex].GetComponent<Tiled2Unity.TiledMap>();
        Instantiate(_levelPrefabs[_currentLevelIndex], _levelPrefabs[_currentLevelIndex].transform.position, Quaternion.identity);
        _initialSpeedSign = tmp.LevelDirection;
        if (BlackOverlay.Instance)
        {
            BlackOverlay.Instance.FadeOut();
            BlackOverlay.Instance.ShowGetReady();
        }
        // Calculate dimensions for level
        _cameraFollower._levelStartX = tmp.transform.position.x;
        _cameraFollower._levelEndX = tmp.transform.position.x + TileUnitSize * tmp.NumTilesWide;
    }
    
	// Use this for initialization
	void Start () {
        _worldText.text = SceneManager.GetActiveScene().buildIndex.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		_coinsText.text = PlayerPrefs.GetInt("CoinCount", 0).ToString();
        _levelText.text = (_currentLevelIndex + 1).ToString();
        
        if(!gameStarted)
        {
#if UNITY_EDITOR
            if(Input.GetMouseButtonDown(0))
#elif UNITY_ANDROID || UNITY_IOS
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
#endif
            {
                if(BlackOverlay.Instance)
                    BlackOverlay.Instance.HideGetReady();
                gameStarted = true;
                SpawnCharacter();
            }
        }
    }

    private void SpawnCharacter()
    {
        Vector3 spawnPos = _characterSpawnPoint.position;
        spawnPos.x = Mathf.Abs(_characterSpawnPoint.position.x) * -_initialSpeedSign;
        GameObject character = Instantiate(_characterPrefab[PlayerPrefs.GetInt("CHARACTER",1)], spawnPos, Quaternion.identity) as GameObject;
        _currCharacter = character.GetComponent<Character>();
        _currCharacter._gameManager = this;
        _currCharacter._moveSpeed = _initialSpeedSign * Mathf.Abs(_currCharacter._moveSpeed);
    }

    public void LoadNextLevel()
    {
        // Save current state
        _currentLevelIndex++;
        int worldIdx = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1;
        PlayerPrefs.SetInt(string.Format("World{0}Level{1}Unlocked", worldIdx, _currentLevelIndex), 1);

        if (_currentLevelIndex < _levelPrefabs.Length)
        {
            if (BlackOverlay.Instance)
            {
                BlackOverlay.Instance.ShowCompleted();
                BlackOverlay.Instance.FadeIn();
            }
            Invoke("ReloadSceneAsync", 1.0f);
        }
        else
        {
            PlayerPrefs.SetInt(string.Format("World{0}Unlocked", UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex), 1);
            GameObject.Find("HUD").transform.Find("NewWorldPanel").gameObject.SetActive(true);
        }
    }

    public void GotoMenu()
    {
        if(BlackOverlay.Instance)
            BlackOverlay.Instance.FadeIn();
        Invoke("GotoMenuAfterDelay", 0.5f);
    }

    public void OpenNextWorld()
    {
        if (BlackOverlay.Instance)
            BlackOverlay.Instance.FadeIn();
        Invoke("OpenNextWorldAfterDelay", 0.5f);
    }

    private void OpenNextWorldAfterDelay()
    {
        int currIdx = SceneManager.GetActiveScene().buildIndex, sceneCount = SceneManager.sceneCountInBuildSettings;
        _currentLevelIndex = 0;
        if(currIdx < sceneCount - 1)
            SceneManager.LoadScene(currIdx + 1);
        else
            SceneManager.LoadScene(0);
    }

    private void GotoMenuAfterDelay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void ReloadSceneAsync()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }



}
