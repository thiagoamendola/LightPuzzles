using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {

	public Slide resetMenu;
	public GameObject[] EngObjs; 
	public GameObject[] PtObjs; 

	void Start(){
		Debug.Log(Application.systemLanguage);
		if(Application.systemLanguage == SystemLanguage.English ||
		Application.platform == RuntimePlatform.WebGLPlayer){
			TranslateEnglish();
		}
	}

	public void TranslateEnglish(){
		foreach(GameObject g in EngObjs){
			g.SetActive(true);
		}
		foreach(GameObject g in PtObjs){
			g.SetActive(false);
		}
	}

	public void StartGame(){
		Application.LoadLevel("Select");
		
	}

	public void ResetProgress(){
		Debug.Log("RESETou!!");
		PlayerPrefs.SetInt("LastSolvedLevel", 1);
	}

	public void Mute(){
		SoundController.instance.ToggleMute();
	}

	public void OpenGooglePlay(){
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.Gamux.LightPuzzles");
	}
}
