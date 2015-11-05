using UnityEngine;
using System.Collections;

public class RotateMirror : MonoBehaviour {


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

		float appliedAngle = trueAngle/2 + initialRotation;
		if(appliedAngle > 90) //Para facilitar os cálculos
			appliedAngle -= 180;

		Debug.Log(appliedAngle);

		if(appliedAngle < 0)
			appliedAngle = 0;
		if(appliedAngle > 50)
			appliedAngle = 50;

		transform.rotation = Quaternion.Euler(0,0,appliedAngle);

	}
}
