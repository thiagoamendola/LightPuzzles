using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameLevelManager2 : MonoBehaviour {

	public bool loadLevel = true;
	public Slide tuto;
	public Slide levelIntro;
	public Slide nextLevelMenu;
	public Slide itsOver;
	public GameObject returnButton;
	public GameObject[] levels;
	GameObject levelInstance = null;
	public Text levelText;
	GameObject UI;

	int level=0;
	Color block1;
	Color block2;
	Color block3;
	Color block4;


	// Use this for initialization
	void Start () {
		
		block1 = new Color(0.2431373f,0.509804f,0.227451f);
		block2 = new Color(0f,0.2509804f,0.4862745f);
		block3 = new Color(0.8247059f,0.522353f,0.0358824f);
		block4 = new Color(0.3431373f,0.1f,0.2843137f);

		level =  PlayerPrefs.GetInt("OpenLevel");
		//Debug.Log(level);

		UI = GameObject.Find("Canvas");

		returnButton.SetActive(true);
		
		if(loadLevel)
			if(level==0){
				StartTuto();
			}else{
				OpenLevel(levels[level]);
			}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void EndLevel(){
		//Salvar progresso
		//Debug.Log(" --> Max level atu = " + PlayerPrefs.GetInt("LastSolvedLevel") + " and " + (level));
		if(level+2 > PlayerPrefs.GetInt("LastSolvedLevel")){
			Debug.Log("  #> mUDOU PARA " + (level+2));
			PlayerPrefs.SetInt("LastSolvedLevel", level+2);
		}

		if(level < levels.Length - 1)
			nextLevelMenu.SlideIn();
		else
			itsOver.GetComponent<Slide>().SlideIn();
	}

	public void NextLevel(){
		nextLevelMenu.SlideDisappear();
		level+=1;
		OpenLevel(levels[level]);
	}

	public void StartLevel(){
		levelIntro.SlideOut();
	}

	void OpenLevel(GameObject nextLevel){
		//Spawnar level
		GameObject newLevel = Instantiate(nextLevel) as GameObject;
		if(nextLevel != levelInstance)
			Destroy(levelInstance);
		levelInstance = newLevel;

		//Mudar cores
		Color currentBlock;
		if(level<=9){
			currentBlock = block1;
		}else if(level<=24){ 
			currentBlock = block2;
		}else if(level<=39){ 
			currentBlock = block3;
		}else{
			currentBlock = block4;
		}
		levelIntro.transform.GetComponent<UnityEngine.UI.Image>().color = currentBlock;
		nextLevelMenu.transform.GetComponent<UnityEngine.UI.Image>().color = currentBlock;

		//Começar intro
		levelText.text = "Nível " + (level+1);
		levelIntro.SlideAppear();
	}

	public void StartTuto(){
		//GetComponent<ResetTimer>().enabled = true;	
		// titleScreen.GetComponent<Slide>().SlideOut();
		//ChangeLevel(levelInstance);
		tuto.SlideIn();
	}

	public void StartFirstLevel(){
		tuto.SlideDisappear();
		OpenLevel(levels[0]);
	}

	public void Restart(){
		Application.LoadLevel("Select");
	}

	public void RestartLevel(){
		PlayerPrefs.SetInt("OpenLevel", level);
		Application.LoadLevel("Game");
	}

	public void TakePhoto(){
		StartCoroutine("TakingPhoto");
	}

	IEnumerator TakingPhoto(){
		UI.SetActive(false);
		string name = "lightpuzzles_" + System.DateTime.Now.Day + "_" + System.DateTime.Now.Month + "_" + 
			System.DateTime.Now.Year + "_" + System.DateTime.Now.ToString("HH") + "_" + 
			System.DateTime.Now.Minute + "_" + System.DateTime.Now.Second;
		print(name);
		Application.CaptureScreenshot(name + ".png");
		yield return null; //wait for a frame
		UI.SetActive(true);
	}

	public void Mute(){
		SoundController.instance.ToggleMute();
	}
}
