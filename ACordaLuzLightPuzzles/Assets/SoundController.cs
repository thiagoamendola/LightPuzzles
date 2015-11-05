using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	public bool mute = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Mute(){
		mute = !mute;

		foreach( AudioSource a in UnityEngine.Object.FindObjectsOfType<AudioSource>())
			a.mute = mute;
	}
}
