using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    public GameManager _gameManager;
    public bool _followHorizontally = false;

    [HideInInspector]
    public float _levelStartX = 0;
    [HideInInspector]
    public float _levelEndX = 0;

    public int _startTileCount = 5;
    public int _endTileCount = 12;

	// Use this for initialization
	void Start () {
      //  GetComponent<AudioSource>().volume = (MainMenu._musicState ? 1 : 0);
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (!_gameManager._currCharacter || float.IsNaN(_gameManager._currCharacter._groundedPosY) || !_gameManager._currCharacter.GetComponent<SpriteRenderer>().enabled)
            return;

        // Follow on y axis
        Transform charTrans = _gameManager._currCharacter.transform;
        float yOffset = charTrans.transform.position.y - _gameManager._currCharacter._groundedPosY;
        Vector3 currPos = transform.position;
        currPos.y = yOffset;
        // positive direction
        bool moveOnPositive = Mathf.Sign(_gameManager._currCharacter._moveSpeed) > 0 && 
            _gameManager._currCharacter.transform.position.x >= _levelStartX + _startTileCount * GameManager.TileUnitSize &&
            _gameManager._currCharacter.transform.position.x <= _levelEndX - _endTileCount * GameManager.TileUnitSize;
        // opposite direction
        bool moveOnNegative = Mathf.Sign(_gameManager._currCharacter._moveSpeed) < 0 && 
            _gameManager._currCharacter.transform.position.x <= _levelEndX - _startTileCount * GameManager.TileUnitSize &&
            _gameManager._currCharacter.transform.position.x >= _levelStartX + _endTileCount * GameManager.TileUnitSize;
        if (moveOnPositive || moveOnNegative)
            currPos.x += _gameManager._currCharacter._moveSpeed * Time.deltaTime;
        // TODO: handle x offset
        transform.position = currPos;
	}
}
