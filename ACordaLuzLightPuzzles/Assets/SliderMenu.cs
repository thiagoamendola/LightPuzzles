using UnityEngine;
using System.Collections;

public class SliderMenu : MonoBehaviour {

	

	public GameObject[] buttonBlocks;
	public GameObject[] strips;

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

	public float interpSpeed;
	float tweenProgress;

	//SpriteRenderer strip;
	int listIndex;

	// Use this for initialization
	void Start () {
		int i;
		//strip = transform.Find("Strip").GetComponent<SpriteRenderer>();

		currentStripPos = strips[0].transform.position;//Definir currentStripPos;
		nextStripPos = new Vector3(17.6f,0,0) + currentStripPos; //Definir nextStripPos;
		previousStripPos = new Vector3(-17.6f,0,0) + currentStripPos; //Definir previousStripPos;

		currentButtonBlockPos = buttonBlocks[0].transform.position;//Definir currentButtonBlockPos;
		nextButtonBlockPos = new Vector3(17.6f,0,0) + currentButtonBlockPos; //Definir nextButtonBlockPos;
		previousButtonBlockPos = new Vector3(-17.6f,0,0) + currentButtonBlockPos; //Definir previousButtonBlockPos;

		//Pegar última fase passada	
		//listIndex = 0; //============================
		listIndex = PlayerPrefs.GetInt("LastSolvedLevel");
		Debug.Log(listIndex);

		//Começar com as paradas no centro
		strips[listIndex].SetActive(true);
		strips[listIndex].transform.position = currentStripPos;
		buttonBlocks[listIndex].SetActive(true);
		buttonBlocks[listIndex].transform.position = currentButtonBlockPos;
		for(i=1; i<strips.Length; i++){
			strips[i].SetActive(true);
			strips[i].transform.position = nextStripPos;
			buttonBlocks[i].SetActive(true);
			buttonBlocks[i].transform.position = nextButtonBlockPos;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void NextSession(){
		if(listIndex < strips.Length){
			listIndex += 1;
			
			// Mover Strip
			previousStrip = strips[listIndex-1].transform;
	 		nextStrip = strips[listIndex].transform;
	 		previousButtonBlock = buttonBlocks[listIndex-1].transform;
	 		nextButtonBlock = buttonBlocks[listIndex].transform;
	 		StartCoroutine("SlideNext");
			//Slide(strips[listIndex-1].transform,
			//	currentStripPos,previousStripPos,interpSpeed);//mover strips[listIndex-1] para a esquerda
			////////////strip.sprite = stripSprites[listIndex];
			//mover strips[listIndex] da direita para o centro

			// Mover bloco de fases 
			//mover buttonBlocks[listIndex-1] para a esquerda
			//mover buttonBlocks[listIndex] da direita para o centro			
		}
	}

	public void PreviousSession(){
		if(listIndex > 0){
			listIndex -= 1;

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
		while(interpH < 1){
			//interp += interpSpeed;
			interpH += interpSpeed;
			interp = Hermite(0f,1f,interpH);
			previousStrip.position = Vector3.Lerp(currentStripPos,previousStripPos, interp);
			nextStrip.position = Vector3.Lerp(nextStripPos,currentStripPos, interp);
			previousButtonBlock.position = Vector3.Lerp(currentButtonBlockPos,previousButtonBlockPos, interp);
			nextButtonBlock.position = Vector3.Lerp(nextButtonBlockPos,currentButtonBlockPos, interp);
			yield return null; //wait for a frame
		}
	}

	private IEnumerator SlidePrev(){
		float interp = 0, interpH = 0;
		while(interp < 1){
			//interp += interpSpeed;
			interpH += interpSpeed;
			interp = Hermite(0f,1f,interpH);
			previousStrip.position = Vector3.Lerp(currentStripPos,nextStripPos, interp);
			nextStrip.position = Vector3.Lerp(previousStripPos,currentStripPos, interp);
			previousButtonBlock.position = Vector3.Lerp(currentButtonBlockPos,nextButtonBlockPos, interp);
			nextButtonBlock.position = Vector3.Lerp(previousButtonBlockPos,currentButtonBlockPos, interp);
			yield return null; //wait for a frame
		}
	}


	//Interpolação de Hermite (Easy In Out)
	public static float Hermite(float start, float end, float value){
        return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
    }

}
