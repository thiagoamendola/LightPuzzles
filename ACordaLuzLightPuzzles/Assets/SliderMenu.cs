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

	SpriteRenderer mainStrip;
	SpriteRenderer btnStrip;
	Color block1;
	Color block2;
	Color block3;
	Color block4;

	public float interpSpeed;
	float tweenProgress;

	//SpriteRenderer strip;
	int listIndex;
	int lastLevel;
	bool blockButtons;

	// Use this for initialization
	void Start () {
		int i, j;
		blockButtons = false;

		block1 = new Color(0.2431373f,0.509804f,0.227451f);
		block2 = new Color(0f,0.2509804f,0.4862745f);
		block3 = new Color(0.8247059f,0.522353f,0.0358824f);
		block4 = new Color(0.3431373f,0.1f,0.2843137f);


		currentStripPos = strips[0].transform.position;//Definir currentStripPos;
		nextStripPos = new Vector3(17.6f,0,0) + currentStripPos; //Definir nextStripPos;
		previousStripPos = new Vector3(-17.6f,0,0) + currentStripPos; //Definir previousStripPos;

		currentButtonBlockPos = buttonBlocks[0].transform.position;//Definir currentButtonBlockPos;
		nextButtonBlockPos = new Vector3(17.6f,0,0) + currentButtonBlockPos; //Definir nextButtonBlockPos;
		previousButtonBlockPos = new Vector3(-17.6f,0,0) + currentButtonBlockPos; //Definir previousButtonBlockPos;

		//Pegar última fase passada	
		
		//PlayerPrefs.SetInt("LastSolvedLevel", 10); //Resetar valores
		lastLevel = PlayerPrefs.GetInt("LastSolvedLevel");
		lastLevel = 56;//*****
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

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void NextSession(){
		if(!blockButtons && listIndex < strips.Length-1){
			listIndex += 1;
			blockButtons = true;
			// Mover Strip
			previousStrip = strips[listIndex-1].transform;
	 		nextStrip = strips[listIndex].transform;
	 		previousButtonBlock = buttonBlocks[listIndex-1].transform;
	 		nextButtonBlock = buttonBlocks[listIndex].transform;
	 		StartCoroutine("SlideNext");
				
		}
	}

	public void PreviousSession(){
		if(!blockButtons && listIndex > 0){
			listIndex -= 1;
			blockButtons = true;

			// Mover Strip
			previousStrip = strips[listIndex+1].transform;
	 		nextStrip = strips[listIndex].transform;
	 		previousButtonBlock = buttonBlocks[listIndex+1].transform;
	 		nextButtonBlock = buttonBlocks[listIndex].transform;
	 		StartCoroutine("SlidePrev");
			//mover strips[listIndex+1] para a direita
			////////////strip.sprite = stripSprites[listIndex];
			//mover strips[listIndex] da esquerda para o centro

			// Mover bloco de fases 
			//mover buttonBlocks[listIndex+1] para a direita
			//mover buttonBlocks[listIndex] da esquerda para o centro			
		}
	}

	private IEnumerator SlideNext(){
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

		while(interpH < 1){
			//Interpolar strips e botões
			interpH += interpSpeed;
			interp = Hermite(0f,1f,interpH);
			previousStrip.position = Vector3.Lerp(currentStripPos,previousStripPos, interp);
			nextStrip.position = Vector3.Lerp(nextStripPos,currentStripPos, interp);
			previousButtonBlock.position = Vector3.Lerp(currentButtonBlockPos,previousButtonBlockPos, interp);
			nextButtonBlock.position = Vector3.Lerp(nextButtonBlockPos,currentButtonBlockPos, interp);
			//Mudar cor
			mainStrip.color = Color.Lerp(currentColor, nextColor, interp);
			btnStrip.color = Color.Lerp(currentColor, nextColor, interp);

			yield return null; //wait for a frame
		}
		blockButtons = false;
	}

	private IEnumerator SlidePrev(){
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

		while(interp < 1){
			//interp += interpSpeed;
			interpH += interpSpeed;
			interp = Hermite(0f,1f,interpH);
			previousStrip.position = Vector3.Lerp(currentStripPos,nextStripPos, interp);
			nextStrip.position = Vector3.Lerp(previousStripPos,currentStripPos, interp);
			previousButtonBlock.position = Vector3.Lerp(currentButtonBlockPos,nextButtonBlockPos, interp);
			nextButtonBlock.position = Vector3.Lerp(previousButtonBlockPos,currentButtonBlockPos, interp);
			//Mudar cor
			mainStrip.color = Color.Lerp(currentColor, nextColor, interp);
			btnStrip.color = Color.Lerp(currentColor, nextColor, interp);

			yield return null; //wait for a frame
		}
		blockButtons = false;
	}


	//Interpolação de Hermite (Easy In Out)
	public static float Hermite(float start, float end, float value){
        return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
    }

    public void GoTitle(){
		Application.LoadLevel("Title");
	}

}
