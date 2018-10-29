using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public int LevelNumber;
	public Switch[] levelSwitches;
	
	bool gameOver;
	
	// Use this for initialization
	void Start () {
		gameOver = false;
		//GameObject.Find("!GameLevelsManager").GetComponent<GameLevelsManager>().SetCurrentLevel(this.gameObject);
		//LevelNumber = PlayerPrefs.GetInt("OpenLevel");
	}
	
	// Update is called once per frame
	void Update () {
		int i;
		bool finished = true;

		if(!gameOver){
			for(i=0; i < levelSwitches.Length; i++){
				if(!levelSwitches[i].activated)
					finished = false;
			}
			
			foreach(MovableObject obj in GetComponentsInChildren<MovableObject>()){
				if(obj.dragging)
					finished = false;
			}
								
			if(finished){
				//Terminar o jogo
				Debug.Log("Tu ganho!!!");
				gameOver = true;
				foreach(MovableObject obj in GetComponentsInChildren<MovableObject>()){
					obj.SetDraggable(false);
				}

				GameObject.Find("!GameLevelManager").GetComponent<GameLevelManager2>().EndLevel();				

			}
		}
	}


}
