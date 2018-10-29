using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLevelsManager : MonoBehaviour {

	public Slide titleScreen;
	public Slide tuto;
	public Slide levelIntro;
	public Slide nextLevelMenu;
	public Slide itsOver;
	public GameObject returnButton;
	public GameObject currentLevel = null;
	public Text levelText;

	// Use this for initialization
	void Awake () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetCurrentLevel(GameObject level){
		currentLevel = level;
	}

	public void EndLevel(){
/*		if(currentLevel.GetComponent<LevelManager>().nextLevel != null)
			nextLevelMenu.SlideIn();
		else
			itsOver.GetComponent<Slide>().SlideIn();*/
	}

	public void NextLevel(){
		nextLevelMenu.SlideDisappear();
		//ChangeLevel(currentLevel.GetComponent<LevelManager>().nextLevel);
	}

	public void StartLevel(){
		levelIntro.SlideOut();
	}

	void ChangeLevel(GameObject Level){
		GameObject newLevel = Instantiate(Level) as GameObject;
		if(Level != currentLevel)
			Destroy(currentLevel);
		currentLevel = newLevel;
		//nextLevelMenu.SetActive(false);
		levelText.text = "Nível " + currentLevel.GetComponent<LevelManager>().LevelNumber;
		levelIntro.SlideAppear();
	}

	public void StartTuto(){
		GetComponent<ResetTimer>().enabled = true;
		returnButton.SetActive(true);
		titleScreen.GetComponent<Slide>().SlideOut();
		//ChangeLevel(currentLevel);
		tuto.SlideIn();
	}

	public void StartFirstLevel(){
		tuto.SlideDisappear();
		ChangeLevel(currentLevel);
	}

	public void Restart(){
		Application.LoadLevel("Select");
	}
}
