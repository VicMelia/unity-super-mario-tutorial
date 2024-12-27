using UnityEngine;
using System.Collections;

public class BorrarPreferencias : MonoBehaviour {

	// Use this for initialization
	void Awake () {

		PlayerPrefs.DeleteAll();
	
	}
	

}
