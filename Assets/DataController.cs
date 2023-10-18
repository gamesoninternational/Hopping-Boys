using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DataController : MonoBehaviour {
    public RoundData[] allRoundData;
    private string gameDataFileName = "data.json";
    public bool IsOnline = false;
    public string jsonString;
    public string JsonURL="http://www.insertnamehere.com/data.json";
    public Image[] ButtonImage;
    public Sprite[] Images;
    private bool finishedLoadingData = false;
	// Use this for initialization

    IEnumerator Start()
    {
           
        WWW www = new WWW(JsonURL);
        yield return www;
        jsonString = www.text;
        LoadGameData();
        for (int loop = 0; loop < allRoundData.Length; loop++) {
            www = new WWW(allRoundData[loop].URLImage);
            yield return www;
            Images[loop] = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0)); 
         }
        finishedLoadingData = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (finishedLoadingData)
        {
            for (int loop = 0; loop < ButtonImage.Length; loop++)
            {
                ButtonImage[loop].sprite = Images[loop];
            }
        }
    }

    private void LoadGameData()
    {
        if (!IsOnline)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);
            if (File.Exists(filePath))
            {
                string dataAsJSON = File.ReadAllText(filePath);
                GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJSON);
                allRoundData = loadedData.allRoundData;
            }
            else
            {
                Debug.LogError("Cannot load json");
            }
        }
        if (IsOnline)
        {
            GameData loadedData = JsonUtility.FromJson<GameData>(jsonString);
            allRoundData = loadedData.allRoundData;
        }
      
    }
    public void ClickedAd(int IndexNum)
    {
        Application.OpenURL(allRoundData[IndexNum].URLPlaystore);
    }

}
