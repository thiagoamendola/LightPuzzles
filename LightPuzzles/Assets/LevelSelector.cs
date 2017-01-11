using UnityEngine;
using System.Collections;

public class LevelSelector : MonoBehaviour {

	public int level;
	public Sprite locked;
	public Sprite current;
	public Sprite cleared;

	Button btn;
	SpriteRenderer sprRend;

	// Use this for initialization
	void Awake () {
		btn = GetComponent<Button>();
		sprRend = GetComponent<SpriteRenderer>();

		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LockLevel(){
		btn.enabled = false;
		sprRend.sprite = locked;
		//Debug.Log("Locked");
	}

	public void CurrentLevel(){
		btn.enabled = true;
		sprRend.sprite = current;
		//Debug.Log("Current");
	}

	public void ClearedLevel(){
		btn.enabled = true;
		sprRend.sprite = cleared;
		//Debug.Log("Done");
	}

	public void LoadLevel(){
		GameObject.Find("BG").transform.Find("Loading").gameObject.SetActive(true);
		PlayerPrefs.SetInt("OpenLevel", level-1);
		Application.LoadLevel("Game");
	}
}
