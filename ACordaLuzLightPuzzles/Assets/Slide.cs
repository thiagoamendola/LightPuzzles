using UnityEngine;
using System.Collections;

public class Slide : MonoBehaviour {
	Vector3 destination;
	protected float progress;
	public bool useLerp = true;
	public float lerpSpeed = 0.05f;
	bool activated;
	bool passed;
	//public bool aspectStretch = false;

	
	// Use this for initialization
	protected virtual void Awake () {
		destination = GetComponent<RectTransform>().localPosition;
		progress = 0;
		passed = false;
		activated = false;

		/*if(GameObject.Find("Camera").GetComponent<Camera>().aspect  - (4f/3f) < 0.01f
		  // && aspectStretch){
			GetComponent<RectTransform>().localScale = new Vector3(.76f, .76f,.76f);
		}else{
			GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
		}*/
		
		//SlideIn();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if(useLerp){
			GetComponent<RectTransform>().localPosition = Vector3.Lerp(GetComponent<RectTransform>().localPosition, destination, progress);
			if(progress < 1)
				progress+=lerpSpeed;
		}else{
			progress = 1;
			GetComponent<RectTransform>().localPosition = destination;
		}
	}
	
	public virtual void SlideIn(){
		Debug.Log("wow");
		activated = true;
		progress = 0f;
		destination = new Vector3(0f, 0f, 0f);
		Debug.Log(GetComponent<RectTransform>().localPosition);
		Debug.Log(destination);
	}
	
	public virtual void SlideOut(){
		passed = true;
		activated = false;
		progress = 0f;
		destination = new Vector3(0f, 100f, 0f); 
	}

	public virtual void SlideAppear(){
		activated = true;
		progress = 1f;
		destination = new Vector3(0f, 0f, 0f); 
	}
	
	public virtual void SlideDisappear(){
		passed = true;
		activated = false;
		progress = 1f;
		destination = new Vector3(0f, 100f, 0f); 
	}
	
	public bool hasPassed(){
		return passed;
	}
	
	public bool hasActivated(){
		return activated;
	}
}
