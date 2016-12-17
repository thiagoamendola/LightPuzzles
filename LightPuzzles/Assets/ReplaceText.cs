using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReplaceText : MonoBehaviour {

	[TextArea(5,10)]
	public string textEng;


	public void ChangeToEng(){
		GetComponent<UnityEngine.UI.Text>().text = textEng;
	}

}
