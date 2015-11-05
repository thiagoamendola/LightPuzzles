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
	void Start () {
		btn = GetComponent<Button>();
		sprRend = GetComponent<SpriteRenderer>();

		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LockLevel(){
		btn.enabled = false;
		sprRend.sprite = locked;
	}

	public void CurrentLevel(){
		btn.enabled = true;
		sprRend.sprite = current;
	}

	public void ClearedLevel(){
		btn.enabled = true;
		sprRend.sprite = cleared;
	}

	public void LoadLevel(){
		PlayerPrefs.SetInt("OpenLevel", level);
		Application.LoadLevel("Game");
	}
}
