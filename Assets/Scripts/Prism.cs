using UnityEngine;
using System.Collections;

public class Prism : MonoBehaviour {
	public Sprite normal;
	public Sprite active;
	
	bool enlightened;
	


	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
		if(!enlightened){
			GetComponent<SpriteRenderer>().sprite = normal;		
			TurnLights (false);
		}else {
			GetComponent<SpriteRenderer>().sprite = active;		
			TurnLights (true);
		}
		
		enlightened = false;
		
		
	}
	
	public void React(){
		Debug.Log("ue");
		enlightened = true;
	}
	
	public void TurnLights (bool active){
		foreach(LightCreator l in GetComponentsInChildren<LightCreator>()){
			l.canLight = active;
		}
	}
}
