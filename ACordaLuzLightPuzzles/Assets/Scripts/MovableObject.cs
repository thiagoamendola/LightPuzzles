using UnityEngine;
using System.Collections;

public class MovableObject : MonoBehaviour {

	Vector3 screenPoint, offset;
    [HideInInspector] public bool dragging;
	public bool canDrag = true;

	// Use this for initialization
	void Start () {
		dragging = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown(){
		screenPoint = Camera.main.WorldToScreenPoint(transform.position);
		
		offset = transform.position - Camera.main.ScreenToWorldPoint(
			new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
			
		dragging = true;
	}
	
	void OnMouseDrag(){
		if(canDrag){
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			transform.position = curPosition;
		}
	}
	
	void OnMouseUp(){
		dragging = false;
	}
	
	public void SetDraggable(bool draggable){
		canDrag = draggable;
	}
}
