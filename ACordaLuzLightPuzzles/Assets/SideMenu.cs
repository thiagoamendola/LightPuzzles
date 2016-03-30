using UnityEngine;
using System.Collections;

public class SideMenu : MonoBehaviour {
	
	public GameObject cog;
	public Credits credits;
	public Slide exitMenu;
	public Button button;
	
	public Vector3 openPos;
	public Vector3 closedPos;
	public Vector3 openCogPos;
	public Vector3 closedCogPos;
	
	public float lerpSpeed;
	public float lerpCogSpeed;
	
	public bool open;
	public bool allowExit = true;
	bool openExit;

	float progress;
	float cogProgress;
	
	Vector3 destination;
	Vector3 cogDestination;
	
	void Start(){
		open = false;
		openExit = false;
		progress = 1;
		cogProgress = 1;

		destination = closedPos;
		cogDestination = closedCogPos;
		GetComponent<RectTransform>().anchoredPosition = destination;
		cog.GetComponent<RectTransform>().anchoredPosition = cogDestination;
	}
	
	void Update(){
		//Debug.Log(Input.mousePosition.y/Screen.height);//GetComponent<RectTransform>().anchoredPosition);

		//Fechar se nao clicar dentro do menu
		if(openExit){
			if(Input.GetMouseButtonDown(0) && (Input.mousePosition.y < Screen.height * .440f || Input.mousePosition.y > Screen.height * .749f))
				CancelExit();
		}else if(open && Input.GetMouseButtonDown(0) && Input.mousePosition.x < Screen.width * .717f){
			OnCogClick();
		}


		//Se tiver no exitMenu e clicar fora dele, fecha-lo

		//Lerpar menu e cog
		GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GetComponent<RectTransform>().anchoredPosition, destination, progress);
		cog.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(cog.GetComponent<RectTransform>().anchoredPosition, cogDestination, cogProgress);

		if(progress < 1)
			progress+=lerpSpeed;
		else{
			progress = 1;
			GetComponent<RectTransform>().anchoredPosition = destination;
		}

		if(cogProgress < 1)
			cogProgress+=lerpCogSpeed;
		else{
			cogProgress = 1;
			cog.GetComponent<RectTransform>().anchoredPosition = cogDestination;
		}	




	}


	public void OnCogClick(){
		open = !open;
		progress = 0;
		cogProgress = 0;
		
		if(open){
			destination = openPos;
			cogDestination = openCogPos;
			if(button!=null)
				button.enabled = false;
		}else{
			destination = closedPos;
			cogDestination = closedCogPos;
			if(button!=null)
				button.enabled = true;
		}

	}

	public void OpenExitMenu(){
		if(allowExit){
			exitMenu.SlideIn();
			openExit = true;
		}
	}

	public void CancelExit(){
		openExit = false;
		exitMenu.SlideOut();
		OnCogClick();
    }

	public void Exit(){
		Debug.Log("Ate logo!");
		Application.Quit();
		CancelExit();
	}

	public void Credits(){
		credits.Open();
	}

}
