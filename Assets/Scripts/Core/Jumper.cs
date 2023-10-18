using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour {

    public float _forceToAdd = 0;
    public float _targetDirection = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Player")
        {
            Character ch = other.gameObject.GetComponent<Character>();
            //if (Mathf.Sign(ch._moveSpeed) == _targetDirection)
            ch.Jump(_forceToAdd);
        }
    }

}
