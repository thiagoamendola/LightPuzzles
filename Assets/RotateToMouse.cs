using UnityEngine;
using System.Collections;

public class RotateToMouse : MonoBehaviour {


	float initialRotation;

	// Use this for initialization
	void Start () {
		initialRotation = transform.rotation.eulerAngles.z;
		Debug.Log(initialRotation);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 mousePos = Input.mousePosition;
		float tan = (mousePos.y - pos.y)/(mousePos.x - pos.x);
		float trueAngle = Mathf.Atan(tan) * Mathf.Rad2Deg;
		if(mousePos.x - pos.x < 0){
			trueAngle += 180;
		}

		transform.rotation = Quaternion.Euler(0,0,trueAngle);

	}
}
