using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour {
	private GameObject lolo;
	private GameObject logo;
	private GameObject mainMenu;
	private bool logoCoroutine = false;

	// Use this for initialization
	void Start () {
		lolo = GameObject.Find ("Lolo");
		logo = GameObject.Find ("Logo");
		logo.SetActive (false);
		mainMenu = GameObject.Find ("MainMenu");
		mainMenu.SetActive (false);

		StartCoroutine (ZoomIn (lolo, 3f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator ZoomIn(GameObject o, float time) {
		Vector3 initScale = o.transform.localScale;

		o.transform.localScale = Vector3.zero;
		o.SetActive (true);

		float scale = 0f;


		while (scale < 1f && o.transform.localScale.x < initScale.x) {
			scale += (Time.deltaTime / time);
			o.transform.localScale = new Vector3(scale, scale, scale);

			yield return null;
		}

		if (scale >= 1f) {
			if (!logoCoroutine) {
				logoCoroutine = true;
				StartCoroutine (ZoomIn (logo, 0.5f));
				StartCoroutine (ZoomIn (mainMenu, 0.5f));
			}
		}
	}
}
