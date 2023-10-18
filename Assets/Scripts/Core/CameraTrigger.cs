using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour {

    public bool _enableMove = false;
    public float _targetDirection = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /*void OnTriggerEnter2D(Collider2D other)
    {
        Collider2D col = GetComponent<Collider2D>();
        if(other.tag == "Player")
        {
            if(Mathf.Sign(other.GetComponent<Character>()._moveSpeed) == _targetDirection)
                Camera.main.GetComponent<CameraFollower>()._followHorizontally = _enableMove;
        }
    }*/

}
