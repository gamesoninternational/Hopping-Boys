using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




public class Music : MonoBehaviour {
	private static Music _instance = null;
	public static Music Instance
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

    void Update(){
        Scene namez = SceneManager.GetActiveScene();
        string name = namez.name;
        if(name == "MainScene"){
            Destroy(this.gameObject);
        }

        Debug.Log(name);
    }
}
