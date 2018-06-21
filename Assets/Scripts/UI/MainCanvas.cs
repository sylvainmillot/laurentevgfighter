using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour {
	public AudioSource audioSource;
	public AudioClip mainTheme;
	public AudioClip characterSelection;

	// Use this for initialization
	void Start () {
		audioSource.clip = mainTheme;
		audioSource.Play ();

		GameObject.Find ("StartScreen").gameObject.SetActive (true);
		GameObject.Find ("CharacterSelection").gameObject.SetActive (false);
	}
	
	public void ChangeClip(AudioClip clip) {
		audioSource.Stop ();
		audioSource.clip = clip;
		audioSource.Play ();
	}
}
