using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {

	private AudioSource audioSource;
	public AudioClip startFight;
	private bool launchFight = false;

	public void Start() {
		audioSource = GetComponent<AudioSource> ();

		Global.player1File = "";
		Global.player2File = "";
	}

	public void Update() {
		LaunchFight ();
	}

	public void LaunchFight() {
		if (!audioSource.isPlaying && launchFight) {
			SceneManager.LoadScene (1);
		}
	}

	public void ChoosePlayerOne(string name) {
		if (Global.player1File == "") {
			Global.player1File = name;
			GameObject.Find (name).transform.Find ("Selector1").gameObject.SetActive (true);
		} else {
			Global.player2File = name;
			GameObject.Find (name).transform.Find ("Selector2").gameObject.SetActive (true);
		}



		if (Global.player1File != "" && Global.player2File != "") {
			audioSource.PlayOneShot (startFight);
			launchFight = true;
		}
	}

	public void PlaySound(AudioClip clip) {
		audioSource.PlayOneShot (clip);
	}
}
