using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOverlay : MonoBehaviour {

    private static BlackOverlay _instance = null;

    public float _fadeSpeed = 2.0f;
    private Color _dstColor;
    public UnityEngine.UI.Image _fadeImage;

    public GameObject _getReady;
    public GameObject _tryAgain;
    public GameObject _completed;

    public static BlackOverlay Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

	// Use this for initialization
	void Start () {
        _dstColor = new Color(0, 0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        _fadeImage.color = Color.Lerp(_fadeImage.color, _dstColor, Time.deltaTime * _fadeSpeed);
	}

    public void FadeIn()
    {
        _dstColor = new Color(0, 0, 0, 1);
    }

    public void FadeOut()
    {
        _dstColor = new Color(0, 0, 0, 0);
    }

    // Get ready
    public void ShowGetReady()
    {
        _getReady.SetActive(true);
        _getReady.GetComponent<Animator>().SetTrigger("GoIn");
    }

    public void HideGetReady()
    {
        _getReady.GetComponent<Animator>().SetTrigger("GoOut");
        Invoke("DisableGetReady", 0.3f);
    }

    void DisableGetReady()
    {
        _getReady.SetActive(false);
    }

    // Try again
    public void ShowTryAgain()
    {
        _tryAgain.GetComponent<Animator>().SetTrigger("GoIn");
        _tryAgain.SetActive(true);
        Invoke("HideTryAgain", 0.3f);
    }

    private void HideTryAgain()
    {
        if (!_tryAgain.activeSelf)
            return;

        _tryAgain.GetComponent<Animator>().SetTrigger("GoOut");
        Invoke("DisableTryAgain", 0.25f);
    }

    void DisableTryAgain()
    {
        _tryAgain.SetActive(false);
    }

    // Try again
    public void ShowCompleted()
    {
        _completed.GetComponent<Animator>().SetTrigger("GoIn");
        _completed.SetActive(true);
        Invoke("HideCompleted", 0.3f);
    }

    private void HideCompleted()
    {
        if (!_completed.activeSelf)
            return;

        _completed.GetComponent<Animator>().SetTrigger("GoOut");
        Invoke("DisableCompleted", 0.25f);
    }

    void DisableCompleted()
    {
        _completed.SetActive(false);
    }

}
