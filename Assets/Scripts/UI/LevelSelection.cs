using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour {

	private AudioSource audioSource;
	public AudioClip startFight;
	private bool launchFight = false;

	public void Start() {
		audioSource = GetComponent<AudioSource> ();

		Global.levelFile = "";
	}

	public void Update() {
		LaunchFight ();
	}

	public void LaunchFight() {
		if (!audioSource.isPlaying && launchFight) {
			SceneManager.LoadScene (2);
		}
	}

	public void ChooseLevel(string name) {
		Global.levelFile = name;

		if (Global.levelFile != "") {
			GameObject.Find ("MainCanvas").GetComponent<AudioSource> ().Stop ();
			audioSource.PlayOneShot (startFight);
			launchFight = true;
		}
	}

	public void PlaySound(AudioClip clip) {
		audioSource.PlayOneShot (clip);
	}
}
