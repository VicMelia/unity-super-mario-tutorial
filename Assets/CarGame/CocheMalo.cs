using UnityEngine;
using System.Collections;

public class CocheMalo : MonoBehaviour {

	public GameObject motorCarretera;
	public MotorCarreteras motorCarreterasScript;
	public GameObject coche;

	void Start()
	{
		motorCarretera= GameObject.Find ("MotorCarreteras");
		motorCarreterasScript = motorCarretera.GetComponent<MotorCarreteras>();
		coche= GameObject.Find ("Coche");
	}


	void OnCollisionEnter2D(Collision2D cInfo)
	{
		if(cInfo.gameObject.tag == "Coche")
		{
			Debug.Log ("Entro");
			motorCarreterasScript.SpeedCocheMalo();
			coche.GetComponent<AudioSource>().pitch = 1f;
			this.gameObject.GetComponent<AudioSource>().Play();
		}

	}
	
	void OnCollisionExit2D(Collision2D cInfo)
	{
		if (cInfo.gameObject.tag == "Coche")
		{
			Debug.Log ("Entro");
			motorCarreterasScript.SpeedCarretera();
			coche.GetComponent<AudioSource>().pitch = 1.6f;
		}
	}


}
