using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fundidos : MonoBehaviour {

	public Image imagenFundido;
	// Use this for initialization
	void Start () {
			
		imagenFundido.CrossFadeAlpha(0,0.5f,false);
	}

}
