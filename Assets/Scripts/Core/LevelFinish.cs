using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        Collider2D col = GetComponent<Collider2D>();
        if(other.tag == "Player" && col.enabled)
        {
            col.enabled = false;
            other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //if(BlackOverlay.Instance)
            //    BlackOverlay.Instance.FadeIn();
            Invoke("FinishAfterDelay", 0.5f);
        }
    }

    private void FinishAfterDelay()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().LoadNextLevel();
    }
}
