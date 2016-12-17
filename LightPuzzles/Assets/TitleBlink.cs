using UnityEngine;
using System.Collections;

public class TitleBlink : MonoBehaviour {

	public float vel = 0.02f;
	SpriteRenderer sprite;
	bool fading;

	void Start () {
		sprite = GetComponent<SpriteRenderer>();
		fading = true;
	}
	
	void Update () {
		if(fading){
			Color c = sprite.color;
			c.a -= vel;
			sprite.color = c;
			if(c.a <= 0)
				fading = false;
		}else{
			Color c = sprite.color;
			c.a += vel;
			sprite.color = c;
			if(c.a >= 1)
				fading = true;
		}
	}
}
