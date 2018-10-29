using UnityEngine;
using System.Collections;

public class ResetTimer : MonoBehaviour {

	public float timeToReset = 15f;
	float time = 0;
	public bool enabled = false;

	// Use this for initialization
	void Start () {
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(enabled){
			Debug.Log(time);
			time += Time.deltaTime;

			if(Input.GetMouseButton(0) || 
			   Mathf.Abs(Input.GetAxis("Mouse X")) > 0 ||
			   Mathf.Abs(Input.GetAxis("Mouse Y")) > 0 ||
			   Input.anyKey){
				time = 0;
			}

			if(time >= timeToReset)
				Application.LoadLevel(0);
		}
	}

	public void Enable(){
		enabled = true;
	}

	public void Disable(){
		enabled = false;
    }

	public bool getEnabled(){
		return enabled;
	}
}
