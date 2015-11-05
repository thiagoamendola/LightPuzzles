using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

	public Slide BG;
	public GameObject text;

	public float startingPoint = -20f;
	public float endingPoint = 290f;

	public float speed = .25f;

	public ResetTimer resetTimer;
	public Button btnCred;
	public Button btnMute;
	public Button btnExit;

	bool activate = false;
	bool restartResetTimer = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(activate){
			text.transform.Translate(0,speed, 0);
			Debug.Log(text.GetComponent<RectTransform>().anchoredPosition);
			if(text.GetComponent<RectTransform>().anchoredPosition.y > endingPoint || Input.GetMouseButtonDown(0)){
				Close ();
			}
		}

	}

	public void Open(){
		activate = true;
		if(resetTimer != null){
			if(resetTimer.getEnabled()){
				resetTimer.Disable();
				restartResetTimer = true;
			}else{
				restartResetTimer = false;
			}
		}

		btnCred.enabled = false;
		btnExit.enabled = false;
		btnMute.enabled = false;

		BG.SlideIn();

		Vector3 pos = text.GetComponent<RectTransform>().anchoredPosition;
		pos.y = startingPoint;
		text.GetComponent<RectTransform>().anchoredPosition = pos;
	}

	public void Close(){
		Debug.Log("Fechou!!!!!");
		activate = false;
		if(resetTimer != null){
			if(restartResetTimer)
				resetTimer.Enable();
		}

		btnCred.enabled = true;
		btnExit.enabled = true;
		btnMute.enabled = true;
        
		BG.SlideOut();
		
		Vector3 pos = text.GetComponent<RectTransform>().anchoredPosition;
		pos.y = startingPoint;
		text.GetComponent<RectTransform>().anchoredPosition = pos;
	}
}
