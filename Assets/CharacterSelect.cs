using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterSelect : MonoBehaviour
{
    public GameObject _Menu;
    public Image[] Characters = new Image[3];
    public int CurrentCharacter;

	public static CharacterSelect Instance;
    // Use this for initialization

	void Awake(){
		Instance = this;
	}
    void OnEnable()
    {
        BlackOverlay.Instance.FadeOut();
        CurrentCharacter = PlayerPrefs.GetInt("CHARACTER", 0);
    }
    void Start()
    {
		PlayerPrefs.SetInt (Global.UNLOCK_CHAR+"1", 1);
		PlayerPrefs.SetInt (Global.UNLOCK_CHAR+"2", 1);
		PlayerPrefs.SetInt (Global.UNLOCK_CHAR+"3", 1);
		PlayerPrefs.Save ();
    }

    // Update is called once per frame
    void Update()
    {
        for (int loop=0; loop < Characters.Length; loop++)
        {
            if (loop != PlayerPrefs.GetInt("CHARACTER", 0))
            {
                Characters[loop].gameObject.SetActive(false);
            }
            else
            {
                Characters[loop].gameObject.SetActive(true);
            }
        }
    }
    public void OnBack()
    {
        BlackOverlay.Instance.FadeIn();
        Invoke("BackAfterDelay", 0.5f);
    }
    private void BackAfterDelay()
    {
        gameObject.SetActive(false);
        _Menu.SetActive(true);
    }
    public void OnClickArrow(int ChangeCharacter)
    {
         CurrentCharacter = PlayerPrefs.GetInt("CHARACTER", 0);
        if (CurrentCharacter + ChangeCharacter < Characters.Length && CurrentCharacter + ChangeCharacter >= 0) {
            CurrentCharacter+=ChangeCharacter;
        }
        else if (CurrentCharacter + ChangeCharacter > Characters.Length-1)
        {
            CurrentCharacter = 0;
        }
        else if (CurrentCharacter + ChangeCharacter < 0)
        {
            CurrentCharacter = Characters.Length-1;
        }
        PlayerPrefs.SetInt("CHARACTER", CurrentCharacter);
    }

	public bool IsCurrentCharLocked()
	{
		int locked=PlayerPrefs.GetInt(Global.UNLOCK_CHAR+Global.currentCharId,0);
		return locked == 0;
	}
	public void UnlockChar()
	{
		PlayerPrefs.SetInt (Global.UNLOCK_CHAR+ Global.currentCharId, 1);
		PlayerPrefs.Save ();
		if (Global.currentChar != null) {
			Global.currentChar.UpdateUI ();
		}
	}

}
