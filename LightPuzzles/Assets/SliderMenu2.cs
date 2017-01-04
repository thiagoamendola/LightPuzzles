using UnityEngine;
using System.Collections;

public class SliderMenu2 : MonoBehaviour {

	int MAXSESSIONS = 10;//11
	float SCREENWIDTH = 17.6f;
	int LEVELSPERSESSION = 5;

	Vector3 iniClickPos;
	Vector3 lastMousePos;
	Vector3 mouseDelta;
	public float dragProp= .03f;//Proportion of space dragged by player
	public float interpSpeed=0.04f;//Amount of speed by which, after movement ceases, menu moves to a valid position
	public float dragFriction=0.7f;//After drag is released, multiplier that slows movement

	Transform slidingContainer;
	SpriteRenderer mainStrip;
	SpriteRenderer btnStrip;

	Vector3 previousSlPos;
	Vector3 nextSlPos;
	
	Color block1;
	Color block2;
	Color block3;
	Color block4;

	int listIndex;
	int lastLevel;
	bool blockButtons;
	float inertialVel;

	// Use this for initialization
	void Start () {
		int i;
		blockButtons = false;

		block1 = new Color(0.2431373f,0.509804f,0.227451f);
		block2 = new Color(0f,0.2509804f,0.4862745f);
		block3 = new Color(0.8247059f,0.522353f,0.0358824f);
		block4 = new Color(0.3431373f,0.1f,0.2843137f);

		slidingContainer = transform.Find("SlidingContainer");
		inertialVel = 0;

		//Pegar última fase passada	
		
		//PlayerPrefs.SetInt("LastSolvedLevel", 10); //Resetar valores
		lastLevel = PlayerPrefs.GetInt("LastSolvedLevel");
		//lastLevel = 6;//*****
		//Debug.Log(lastLevel);
		if(lastLevel<1){
			lastLevel = 1;
		}
		listIndex = Mathf.Min((lastLevel-1) / LEVELSPERSESSION,MAXSESSIONS);
		

		//Mudar posições dos botões
		Vector3 slIniPos = slidingContainer.position;
		slIniPos.x = -SCREENWIDTH*listIndex;
		slidingContainer.position = slIniPos;

		//Mudar estados dos botões	
		foreach(LevelSelector l in GetComponentsInChildren<LevelSelector>()){
			//Debug.Log(" ==> " + l.level);
			if(l.level < lastLevel){
				l.ClearedLevel();
			}else if(l.level == lastLevel){
				l.CurrentLevel();
			}else{
				l.LockLevel();
			}
		}
	

		//Mudar cor do bloco
		mainStrip = transform.Find("MainStrip").GetComponent<SpriteRenderer>();
		btnStrip = transform.Find("StripButtons").GetComponent<SpriteRenderer>();
		Color currentBlock;
		if(listIndex<=1){
			currentBlock = block1;
		}else if(listIndex<=4){ 
			currentBlock = block2;
		}else if(listIndex<=7){ 
			currentBlock = block3;
		}else{
			currentBlock = block4;
		}
		mainStrip.color = currentBlock;
		btnStrip.color = currentBlock;

		//Pegar posição do mouse atual
		lastMousePos = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {
		//Aplicar função de segurar e soltar mouse		
		if(!blockButtons)
			DragMenu();
		else if(inertialVel != 0f)
			MoveByInertia(Time.deltaTime);
		
	}

	public void DragMenu(){
		mouseDelta = lastMousePos - Input.mousePosition;
		//Debug.Log(-mouseDelta);
		//Debug.Log(-mouseDelta.x * dragProp);
		if(Input.GetMouseButtonDown(0)){
			//print ("Just pressed");
			iniClickPos = Input.mousePosition;
		}else if (Input.GetMouseButton(0)){
  			if(!blockButtons && Vector3.Distance(iniClickPos, Input.mousePosition)>3f){
				//Dragar todo mundo relativo ao delta do mouse				
				//print ("DRAAAAAAAAG");
				Vector3 dragMove = slidingContainer.position;
				dragMove.x -= mouseDelta.x * dragProp;
				dragMove.x = Mathf.Clamp(dragMove.x, -SCREENWIDTH*MAXSESSIONS, 0f);
				slidingContainer.position = dragMove;	  			
	  			
	  			listIndex = Mathf.RoundToInt(-slidingContainer.position.x/SCREENWIDTH);
	  			StartCoroutine("InterpStripColor");			
			}
		}else if (Input.GetMouseButtonUp(0)){
 			if(Vector3.Distance(iniClickPos, Input.mousePosition)>3f){//Nao eh click 				
 				//print ("Released");
 				inertialVel = mouseDelta.x * dragProp; 				
 				if(inertialVel!=0){
 					blockButtons = true;
 				}else{
 					//Fit listIndex
					listIndex = Mathf.RoundToInt(-slidingContainer.position.x/SCREENWIDTH);
 					//Slide to nearest section
					previousSlPos = slidingContainer.position;
					nextSlPos = previousSlPos;
					nextSlPos.x = -SCREENWIDTH*listIndex;
			 		StartCoroutine("Slide");
 				}
	 		}else{
	 			//print ("Clicked");
	 		}
	 	}

 		lastMousePos = Input.mousePosition;
	}

	//Move sliding container
	public void MoveByInertia(float deltaTime){
		//print("In inertia: " + inertialVel.ToString());
		Vector3 dragMove = slidingContainer.position;
		dragMove.x -= inertialVel;
		
		//Fit listIndex
		listIndex = Mathf.RoundToInt(-dragMove.x/SCREENWIDTH);

		inertialVel *= dragFriction;		
		if(Mathf.Clamp(dragMove.x, -SCREENWIDTH*MAXSESSIONS, 0f) != dragMove.x || //Hit a wall. Stop
			Mathf.Abs(inertialVel)<=0.1f){ //Stopped by drag force
			//Stop			
			dragMove.x = Mathf.Clamp(dragMove.x, -SCREENWIDTH*MAXSESSIONS, 0f);
			inertialVel = 0;
			
			//Slide to nearest section
			previousSlPos = slidingContainer.position;
			nextSlPos = previousSlPos;
			nextSlPos.x = -SCREENWIDTH*listIndex;
	 		StartCoroutine("Slide");	
		}

	 	StartCoroutine("InterpStripColor");			
		slidingContainer.position = dragMove;		
	}


	public void NextSession(){		
		if(!blockButtons && listIndex < MAXSESSIONS){
			listIndex += 1;
			blockButtons = true;
			// Mover container		
			previousSlPos = slidingContainer.position;
			nextSlPos = previousSlPos;
			nextSlPos.x = -SCREENWIDTH*listIndex;
	 		StartCoroutine("Slide");	
	 		StartCoroutine("InterpStripColor");			
		}
	}

	public void PreviousSession(){
		if(!blockButtons && listIndex > 0){
			listIndex -= 1;
			blockButtons = true;
			// Mover container
			previousSlPos = slidingContainer.position;
			nextSlPos = previousSlPos;
			nextSlPos.x = -SCREENWIDTH*listIndex;
	 		StartCoroutine("Slide");
	 		StartCoroutine("InterpStripColor");
		}
	}

	private IEnumerator Slide(){
		float interp = 0, interpH = 0;
		//print("Sliding");

		while(interpH < 1){
			//Interpolar strips e botões
			interpH += interpSpeed;
			interp = Hermite(0f,1f,interpH);
			slidingContainer.position = Vector3.Lerp(previousSlPos,nextSlPos, interp);			
			yield return null; //wait for a frame
		}
		blockButtons = false;		
	}

	private IEnumerator InterpStripColor(){
		print("InterpColor START");
		float interp = 0, interpH = 0;

		//Ver para qual cor mudar
		Color currentColor = mainStrip.color;
		Color nextColor;
		if(listIndex<=1){
			nextColor = block1;
		}else if(listIndex<=4){ 
			nextColor = block2;
		}else if(listIndex<=7){ 
			nextColor = block3;
		}else{
			nextColor = block4;
		}

		if(nextColor != currentColor){
			while(interp < 1){
				//Debug.Log("Mudando cor " + interp);
				//Debug.Log("Mudando interp -> " + interpH);
				interpH = Mathf.Clamp(interpH + interpSpeed, 0f, 1f);
				interp = Hermite(0f,1f,interpH);
				mainStrip.color = Color.Lerp(currentColor, nextColor, interp);
				btnStrip.color = Color.Lerp(currentColor, nextColor, interp);
				yield return null; //wait for a frame
			}
		}
		print("InterpColor END");
	}


	//Interpolação de Hermite (Easy In Out)
	public static float Hermite(float start, float end, float value){
        return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
    }

    public void GoTitle(){
		Application.LoadLevel("Title");
	}

}
