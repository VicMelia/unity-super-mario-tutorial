using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BotonEscenas : MonoBehaviour {

	public Image fundido;

	void OnMouseDown()
	{
		this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.3f,0.3f,0.3f,1);
		this.gameObject.GetComponent<AudioSource>().Play();
	}

	void OnMouseOver()
	{
		this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f,1);
	}

	void OnMouseExit()
	{
		this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
	}

	void OnMouseUp()
	{
		this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
		this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
		fundido.CrossFadeAlpha(1,0.5f,false);
		StartCoroutine(CargoEscena());
	}

	IEnumerator CargoEscena()

	{
		yield return new WaitForSeconds (1);
		Application.LoadLevel("Juego");
	}





}











