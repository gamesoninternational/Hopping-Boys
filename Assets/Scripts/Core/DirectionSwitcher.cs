using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSwitcher : MonoBehaviour {

    public float _targetDirection = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Collider2D col = GetComponent<Collider2D>();
        // Inverse direction
        if(other.tag == "Player")
        {
            Character ch = other.GetComponent<Character>();
            if (Mathf.Sign(ch._moveSpeed) != _targetDirection)
            {
                ch._moveSpeed = Mathf.Abs(ch._moveSpeed) * _targetDirection;
                ch.GetComponent<AudioSource>().PlayOneShot(ch._audioClips[3]);
            }
        }
    }

}
