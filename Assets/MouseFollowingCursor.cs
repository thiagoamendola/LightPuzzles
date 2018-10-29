using UnityEngine;
using System.Collections;

public class MouseFollowingCursor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position = new Vector3(-6.06f, 3.1f,0f);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);

		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
		//Debug.Log(gameObject.name + ": " + transform.position);
		
		//Apply screen limits
		if(curPosition.x > 8.7f){
			curPosition.x = 8.7f;
		}else if(curPosition.x < -8.7f){
			curPosition.x = -8.7f;
		}
		if(curPosition.y > 4.7f){
			curPosition.y = 4.7f;
		}else if(curPosition.y < -4.7f){
			curPosition.y = -4.7f;
		}

		transform.position = new Vector3(transform.position.x,curPosition.y,transform.position.z);
	}
}
