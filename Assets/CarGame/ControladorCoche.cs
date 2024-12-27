using UnityEngine;
using System.Collections;

public class ControladorCoche : MonoBehaviour {

	public GameObject Coche;
	public float velocidadMovimientoCoche;

	//float factor = 3;
	public float anguloDeGiro;

	void FixedUpdate()
	{
		float giroEnEjeZ = 0;

		transform.Translate(Vector3.right * 
		                    Input.GetAxis("Horizontal")*
		                    velocidadMovimientoCoche *
		                    Time.deltaTime
		                    );

		giroEnEjeZ = Input.GetAxis("Horizontal")* -anguloDeGiro;
		Coche.transform.rotation = Quaternion.Euler(0,0,giroEnEjeZ);

		if (Coche.transform.position.y < -4.3f || Coche.transform.position.x < -10.0f
		    || Coche.transform.position.x >10)
		{
			ResetPosition();
		}

	}

	void ResetPosition()
	{
		Coche.transform.position = new Vector3(0f,-2.4f,0f);
	}
}










