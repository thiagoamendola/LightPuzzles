using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {

	public GameObject resetMenu;


	public void StartGame(){
		Application.LoadLevel("Select");
	}

	public void ShowResetMenu(){
		resetMenu.SetActive(true);
	}

	public void HideResetMenu(){
		resetMenu.SetActive(false);
	}

	public void ResetProgress(){
		Debug.Log("REsetou!!");
		PlayerPrefs.SetInt("LastSolvedLevel", 1);
	}

	public void Mute(){
		SoundController.instance.ToggleMute();
	}

	public void OpenGooglePlay(){
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.Gamux.LightPuzzles");
	}
}
