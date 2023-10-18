using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrigger : MonoBehaviour {

    public float _targetTime = 1.0f;
    public float _jumpMiltiplier = 1.2f;
    public float _moveSpeedMultiplier = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Collider2D col = GetComponent<Collider2D>();
        if(other.tag == "Player" && col.enabled)
        {
            col.enabled = false;
            Time.timeScale = _targetTime;
            Character ch = other.GetComponent<Character>();
            ch._jumpForce = ch._initialJumpForce * (_targetTime > 1 ? _jumpMiltiplier : 1); // Adjust jump force
            ch._moveSpeed = Mathf.Sign(ch._moveSpeed) * Mathf.Abs(ch._initialMoveSpeed) * _moveSpeedMultiplier;
            ch.GetComponent<AudioSource>().PlayOneShot(ch._audioClips[(_targetTime > 1 ? 4 : 5)]);
        }
    }
}
