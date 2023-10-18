
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour {

    public float _moveSpeed;
    public float _jumpForce;
    private Rigidbody2D _charBody;
    private bool _isJumping = false;
	public Animator _animator;
    private SpriteRenderer _spriteRenderer;
    [HideInInspector]
    public float _initialJumpForce;
    [HideInInspector]
    public float _initialMoveSpeed;
    [HideInInspector]
    public float _groundedPosY = float.NaN;

    public GameManager _gameManager;

    public AudioClip[] _audioClips;

    private static int _gameOverCounter = 0;
    private static int _adDisplayCounter = 0;
	// Use this for initialization
	void Start () {
        _charBody = GetComponent<Rigidbody2D>();
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialJumpForce = _jumpForce;
        _initialMoveSpeed = _moveSpeed;

        GetComponent<AudioSource>().volume = (MainMenu._sfxState ? 1 : 0);
	}
	// Update is called once per frame
	void Update () {

        // Move character
        _charBody.transform.position += new Vector3(_moveSpeed * Time.deltaTime, 0, 0);
        _spriteRenderer.flipX = (_moveSpeed < 0);

        // Process controls
#if UNITY_ANDROID || UNITY_IOS
        if(Input.touchCount > 0)
        {
            Touch t0 = Input.touches[0];
            if(t0.phase == TouchPhase.Began)
                Jump();
        }
#endif

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
            Jump();
#endif

        _animator.SetBool("IsJumping", _isJumping);
    }

    public void Jump(float overrideForce = 0)
    {
        if ((_isJumping && overrideForce == 0) || EventSystem.current.currentSelectedGameObject != null)
            return;

        _isJumping = true;
        float dstForce = (overrideForce != 0 ? overrideForce : _jumpForce);
        _charBody.velocity = Vector2.zero;
        _charBody.AddForce(new Vector2(0, dstForce), ForceMode2D.Impulse);
        GetComponent<AudioSource>().PlayOneShot(_audioClips[0]);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Reset jump
        if(other.collider.tag == "Ground")
        {
            if (float.IsNaN(_groundedPosY))
                _groundedPosY = transform.position.y;
            if(GetComponent<Rigidbody2D>().velocity.y <= 0)
                _isJumping = false;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Process death
        if(other.tag == "Obstacle" && GetComponent<Collider2D>().enabled)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<AudioSource>().PlayOneShot(_audioClips[2]);

            if (BlackOverlay.Instance)
            {
                BlackOverlay.Instance.FadeIn();
                BlackOverlay.Instance.ShowTryAgain();
            }
            _gameOverCounter++;
            // Show interstitial
            if(AdManager.Instance)
            {
                if (_gameOverCounter % AdManager.Instance._adsDisplayFrequency == 0)
                {
                    _adDisplayCounter++;
                    bool useAdmob = (_adDisplayCounter <= AdManager.Instance._admobDisplayCount);
                    AdManager.Instance.ShowInterstitial(useAdmob);
                    // Reset counter
                    if (!useAdmob)
                        _adDisplayCounter = 0;
                }
            }
            // Load next level
            Invoke("GameOverAfterDelay", 0.5f);
        }
        // Coin pickup
        if(other.tag == "Coin" && other.GetComponent<Collider2D>().enabled)
        {
            other.GetComponent<Collider2D>().enabled = false;
            GetComponent<AudioSource>().PlayOneShot(_audioClips[1]);
            // Play pickup animation
            MoveToUIElem mt = other.gameObject.AddComponent<MoveToUIElem>();
            mt._target = _gameManager._coinsText.transform.parent.GetChild(0).GetComponent<RectTransform>();
            mt._moveSpeed = 30;
            mt._targetScale = new Vector3(0.9f, 0.9f, 0.9f);
            mt._targetColor = new Color(1, 1, 1, 0.5f);
            //Destroy(other.gameObject);
            PlayerPrefs.SetInt("CoinCount", PlayerPrefs.GetInt("CoinCount", 0) + 1);
        }
    }

    private void GameOverAfterDelay()
    {
        Application.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

}
