using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	private AudioSource audioSource;

	public void Start() {
		audioSource = GetComponent<AudioSource> ();
	}

	public void Credits() {
		SceneManager.LoadScene (3);
	}

	public void QuitGame() {
		Debug.Log ("Quit game");

		Application.Quit ();
	}

	public void PlaySound(AudioClip clip) {
		audioSource.PlayOneShot (clip);
	}
}
