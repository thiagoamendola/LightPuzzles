using UnityEngine;
using System.Collections;

public class ResolutionController : MonoBehaviour {

	public static ResolutionController instance = null;

	public float HVGAValue;
	public float WXGAValue;
	public float WVGAValue;
	public float WSVGAValue;
	public float FWVGAValue;

	void Awake(){
		//Singleton
		if(instance == null)
			instance = this;
		else if(this != instance)
				Destroy(gameObject);
		DontDestroyOnLoad(this);
	}
	
	//HVGA => 1.51 <
	//WXGA => 1.57 ~ 1.62
	//WVGA => 1.66 ~ 1.67
	//WSVGA => 1.70 ~ 1.71
	//FWVGA => 1.775 >

	void Update () {
		float proportion = (float)Screen.width/(float)Screen.height;
		Camera camera = GameObject.Find("Camera").GetComponent<Camera>();
		/*if(proportion < 1.51f){
			camera.orthographicSize = HVGAValue;
		}else if(proportion < 1.62f){
			camera.orthographicSize = WXGAValue;
		}else if(proportion < 1.67f){
			camera.orthographicSize = WVGAValue;
		}else if(proportion < 1.71f){
			camera.orthographicSize = WSVGAValue;
		}else{
			camera.orthographicSize = FWVGAValue;
		}*/
		float horizontalSize = 8.757874f;
		camera.orthographicSize = horizontalSize / camera.aspect;
		//Debug.Log(camera.orthographicSize * camera.aspect);		

	}

	// Update is called once per frame
	//void  () {
		//print((float)Screen.width/(float)Screen.height);	
	//}
}
