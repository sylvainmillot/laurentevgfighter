using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour {
	
	void Update () {
		if (Input.GetKey (KeyCode.Escape) || Input.GetKey (KeyCode.KeypadEnter) || Input.GetKey (KeyCode.Return)) {
			SceneManager.LoadScene (0);
		}
	}
}
