using UnityEngine;
using System.Collections;

public class SliderMenu : MonoBehaviour {

	public GameObject[] buttonBlocks;
	public GameObject[] strips;

	int MAXSESSIONS = 10;//11

	Vector3 currentStripPos;
	Vector3 nextStripPos;
	Vector3 previousStripPos;

	Vector3 currentButtonBlockPos;
	Vector3 nextButtonBlockPos;
	Vector3 previousButtonBlockPos;

	Transform previousStrip;
	Transform nextStrip;
	Transform previousButtonBlock;
	Transform nextButtonBlock;
	GameObject dummy;

	SpriteRenderer mainStrip;
	SpriteRenderer btnStrip;
	Color block1;
	Color block2;
	Color block3;
	Color block4;

	Vector3 iniClickPos;
	Vector3 lastMousePos;
	Vector3 mouseDelta;
	public float dragProp= .05f;//Proportion

	public float interpSpeed;
	float tweenProgress;

	//SpriteRenderer strip;
	int listIndex;
	int lastLevel;
	bool blockButtons;

	// Use this for initialization
	void Start () {
		int i;
		blockButtons = false;

		block1 = new Color(0.2431373f,0.509804f,0.227451f);
		block2 = new Color(0f,0.2509804f,0.4862745f);
		block3 = new Color(0.8247059f,0.522353f,0.0358824f);
		block4 = new Color(0.3431373f,0.1f,0.2843137f);


		currentStripPos = strips[0].transform.position;//Definir currentStripPos;
		nextStripPos = new Vector3(17.6f,0,0) + currentStripPos; //Definir nextStripPos;
		previousStripPos = new Vector3(-17.6f,0,0) + currentStripPos; //Definir previousStripPos;
		print(previousStripPos);
		currentButtonBlockPos = buttonBlocks[0].transform.position;//Definir currentButtonBlockPos;
		nextButtonBlockPos = new Vector3(17.6f,0,0) + currentButtonBlockPos; //Definir nextButtonBlockPos;
		previousButtonBlockPos = new Vector3(-17.6f,0,0) + currentButtonBlockPos; //Definir previousButtonBlockPos;

		dummy = new GameObject();

		//Pegar última fase passada	
		
		//PlayerPrefs.SetInt("LastSolvedLevel", 10); //Resetar valores
		lastLevel = PlayerPrefs.GetInt("LastSolvedLevel");
		//lastLevel = 56;//*****
		//Debug.Log(lastLevel);
		if(lastLevel<1){
			lastLevel = 1;
		}
		listIndex = Mathf.Min((lastLevel-1) / 5,MAXSESSIONS);
		

		//Mudar posições dos botões
		for(i=0; i<strips.Length; i++){
			strips[i].SetActive(true);
			buttonBlocks[i].SetActive(true);
			if(i < listIndex){
				strips[i].transform.position = previousStripPos;
				buttonBlocks[i].transform.position = previousButtonBlockPos;
			}else if(i==listIndex){
				strips[listIndex].transform.position = currentStripPos;
				buttonBlocks[listIndex].transform.position = currentButtonBlockPos;
			}else{
				strips[i].transform.position = nextStripPos;
				buttonBlocks[i].transform.position = nextButtonBlockPos;
			}
		}

		//Mudar estados dos botões
		for(i=0; i<buttonBlocks.Length; i++){
			foreach(LevelSelector l in buttonBlocks[i].GetComponentsInChildren<LevelSelector>()){
				//Debug.Log(" ==> " + l.level);
				if(l.level < lastLevel){
					l.ClearedLevel();
				}else if(l.level == lastLevel){
					l.CurrentLevel();
				}else{
					l.LockLevel();
				}
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
		DragMenu();
	}

	public void DragMenu(){
		mouseDelta = lastMousePos - Input.mousePosition;
		//Debug.Log(mouseDelta);
		if(Input.GetMouseButtonDown(0)){
			//print ("Just pressed");
			iniClickPos = Input.mousePosition;
		}else if (Input.GetMouseButton(0)){
  			//print ("Pressed");
  			if(!blockButtons && Vector3.Distance(iniClickPos, Input.mousePosition)>3f){
				//Dragar todo mundo relativo ao delta do mouse
	  			buttonBlocks[listIndex].transform.Translate(-mouseDelta.x * dragProp,0,0);
	  			strips[listIndex].transform.Translate(-mouseDelta.x * dragProp,0,0);
	  			if(listIndex < strips.Length-1){	  				
	  				if(!(-mouseDelta.x > 0 && strips[listIndex+1].transform.position.x >= 17.6f) && //forward too much
	  					!(-mouseDelta.x < 0 && strips[listIndex].transform.position.x > 0)){ //backward when not allowed
	  					float newPos = -mouseDelta.x * dragProp;
	  					if(newPos > 0)
	  						newPos = Mathf.Min(-mouseDelta.x * dragProp, 17.6f - buttonBlocks[listIndex+1].transform.position.x);		  			
	  					buttonBlocks[listIndex+1].transform.Translate(newPos,0,0);
	  					strips[listIndex+1].transform.Translate(newPos,0,0);
	  				}
	  			}
	  			if(listIndex > 0){ //previous
	  				if(!(-mouseDelta.x < 0 && strips[listIndex-1].transform.position.x <= -17.6f) && //backward too much
	  					!(-mouseDelta.x > 0 && strips[listIndex].transform.position.x < 0)){ //forward when not allowed
	  					float newPos = -mouseDelta.x * dragProp;
	  					if(newPos < 0)
	  						newPos = -Mathf.Min(Mathf.Abs(-mouseDelta.x * dragProp), Mathf.Abs(-17.6f - buttonBlocks[listIndex-1].transform.position.x));
	  					buttonBlocks[listIndex-1].transform.Translate(newPos,0,0);
	  					strips[listIndex-1].transform.Translate(newPos,0,0);	  					  				
	  				}
	  			}
	  			//DEIXAR AQUI MAIS ESPERTO
	  			//Fazer com que blocos não sejam arremessados muito longe
	  			//Atualizar listIndex e mover possíveis novos blocos
	  			//print(strips[listIndex].transform.position.x);
	  			if(strips[listIndex].transform.position.x < -17.6f && listIndex < strips.Length-1){
	 				//NEXT	 				
	 				print("NEXT");
        	 		listIndex++;
			 		StartCoroutine("InterpStripColor");
	 			}else if(strips[listIndex].transform.position.x > 17.6f && listIndex > 0){
	 				//PREVIOUS
	 				print("PREVIOUS");
	 				listIndex--;
			 		StartCoroutine("InterpStripColor");
	 			}

  			 }

		}else if (Input.GetMouseButtonUp(0)){
 			//print ("Released");
 			if(Vector3.Distance(iniClickPos, Input.mousePosition)>3f){//Nao eh click
 				//print(strips[listIndex].transform.position.x);
 				blockButtons = true;
	 			if(strips[listIndex].transform.position.x < -17.6f/2 && listIndex < strips.Length-1){
	 				//NEXT
	 				listIndex++;

	 				previousStrip = strips[listIndex-1].transform;
			 		previousButtonBlock = buttonBlocks[listIndex-1].transform;
			 		nextStrip = strips[listIndex].transform;
			 		nextButtonBlock = buttonBlocks[listIndex].transform;
			 		StartCoroutine("SlideNext");				
			 		StartCoroutine("InterpStripColor");
	 			}else if(strips[listIndex].transform.position.x > 17.6f/2 && listIndex > 0){
	 				//PREVIOUS
					listIndex--;

					previousStrip = strips[listIndex+1].transform;
			 		previousButtonBlock = buttonBlocks[listIndex+1].transform;
			 		nextStrip = strips[listIndex].transform;
			 		nextButtonBlock = buttonBlocks[listIndex].transform;
			 		StartCoroutine("SlidePrev");
			 		StartCoroutine("InterpStripColor");
	 			}else{
	 				//Retornar para posição normal
	 				if(strips[listIndex].transform.position.x < 0){
	 					if(listIndex < strips.Length-1){
	 						previousStrip = strips[listIndex+1].transform;
				 			previousButtonBlock = buttonBlocks[listIndex+1].transform;
				 		}else{
							previousStrip = dummy.transform;
			 				previousButtonBlock = dummy.transform; 			
			 			}
				 		nextStrip = strips[listIndex].transform;
				 		nextButtonBlock = buttonBlocks[listIndex].transform;
				 		StartCoroutine("SlidePrev");
				 		StartCoroutine("InterpStripColor");
					}else{
						if(listIndex > 0){
		 					previousStrip = strips[listIndex-1].transform;
				 			previousButtonBlock = buttonBlocks[listIndex-1].transform;
				 		}else{
							previousStrip = dummy.transform;
			 				previousButtonBlock = dummy.transform; 			
			 			}
				 		nextStrip = strips[listIndex].transform;
				 		nextButtonBlock = buttonBlocks[listIndex].transform;
				 		StartCoroutine("SlideNext");	
				 		StartCoroutine("InterpStripColor");
					}
	 			}
			
	 		}
	 	}
	 	
 		lastMousePos = Input.mousePosition;
	}

	public void NextSession(){
		if(!blockButtons && listIndex < strips.Length-1){
			listIndex += 1;
			blockButtons = true;
			// Mover Strip
			previousStrip = strips[listIndex-1].transform;
	 		previousButtonBlock = buttonBlocks[listIndex-1].transform;
	 		nextStrip = strips[listIndex].transform;
	 		nextButtonBlock = buttonBlocks[listIndex].transform;
	 		StartCoroutine("SlideNext");	
	 		StartCoroutine("InterpStripColor");			
		}
	}

	public void PreviousSession(){
		if(!blockButtons && listIndex > 0){
			listIndex -= 1;
			blockButtons = true;
			// Mover Strip
			previousStrip = strips[listIndex+1].transform;
	 		previousButtonBlock = buttonBlocks[listIndex+1].transform;
	 		nextStrip = strips[listIndex].transform;
	 		nextButtonBlock = buttonBlocks[listIndex].transform;
	 		StartCoroutine("SlidePrev");
	 		StartCoroutine("InterpStripColor");
		}
	}

	private IEnumerator SlideNext(){
		float interp = 0, interpH = 0;

		while(interpH < 1){
			//Interpolar strips e botões
			interpH += interpSpeed;
			interp = Hermite(0f,1f,interpH);
			previousStrip.position = Vector3.Lerp(previousStrip.position,previousStripPos, interp);
			nextStrip.position = Vector3.Lerp(nextStrip.position,currentStripPos, interp);
			previousButtonBlock.position = Vector3.Lerp(previousButtonBlock.position,previousButtonBlockPos, interp);
			nextButtonBlock.position = Vector3.Lerp(nextButtonBlock.position,currentButtonBlockPos, interp);
			yield return null; //wait for a frame
		}
		blockButtons = false;
	}

	private IEnumerator SlidePrev(){
		float interp = 0, interpH = 0;

		while(interp < 1){			
			interpH += interpSpeed;
			interp = Hermite(0f,1f,interpH);
			previousStrip.position = Vector3.Lerp(previousStrip.position,nextStripPos, interp);
			nextStrip.position = Vector3.Lerp(nextStrip.position,currentStripPos, interp);
			previousButtonBlock.position = Vector3.Lerp(previousButtonBlock.position,nextButtonBlockPos, interp);
			nextButtonBlock.position = Vector3.Lerp(nextButtonBlock.position,currentButtonBlockPos, interp);
			yield return null; //wait for a frame
		}
		blockButtons = false;
	}

	private IEnumerator InterpStripColor(){
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
				//interp += interpSpeed;
				interpH += interpSpeed;
				interp = Hermite(0f,1f,interpH);
				mainStrip.color = Color.Lerp(currentColor, nextColor, interp);
				btnStrip.color = Color.Lerp(currentColor, nextColor, interp);
				yield return null; //wait for a frame
			}
		}
	}


	//Interpolação de Hermite (Easy In Out)
	public static float Hermite(float start, float end, float value){
        return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
    }

    public void GoTitle(){
		Application.LoadLevel("Title");
	}

}
