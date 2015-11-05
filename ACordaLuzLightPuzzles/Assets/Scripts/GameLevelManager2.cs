using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLevelManager2 : MonoBehaviour {

	public Slide tuto;
	public Slide levelIntro;
	public Slide nextLevelMenu;
	public Slide itsOver;
	public GameObject returnButton;
	public GameObject[] levels;
	GameObject levelInstance = null;
	public Text levelText;

	int level=0;

	// Use this for initialization
	void Start () {
		
		level =  PlayerPrefs.GetInt("OpenLevel");
		//Debug.Log(level);

		returnButton.SetActive(true);

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
		GameObject newLevel = Instantiate(nextLevel) as GameObject;
		if(nextLevel != levelInstance)
			Destroy(levelInstance);
		levelInstance = newLevel;
		//nextLevelMenu.SetActive(false);
		levelText.text = "Nível " + levelInstance.GetComponent<LevelManager>().LevelNumber;
		levelIntro.SlideAppear();
	}

	public void StartTuto(){
		//GetComponent<ResetTimer>().enabled = true;	
		// titleScreen.GetComponent<Slide>().SlideOut();
		//ChangeLevel(levelInstance);
		Debug.Log("Chegou aqui");
		tuto.SlideIn();
	}

	public void StartFirstLevel(){
		tuto.SlideDisappear();
		OpenLevel(levels[0]);
	}

	public void Restart(){
		Application.LoadLevel("Select");
	}
}
