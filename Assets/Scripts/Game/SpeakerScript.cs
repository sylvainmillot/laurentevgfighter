using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SpeakerScript : MonoBehaviour {
	private AudioSource audioSource;
	private AudioClip currentClip;
	private AudioClip[] clips; 

	public AudioClip fight;
	public AudioClip round;
	public AudioClip one;
	public AudioClip two;
	public AudioClip three;
	public AudioClip final;
	public AudioClip perfect;
	public AudioClip win;
	public AudioClip loose;
	public AudioClip you;

	private Text fightText;
	private Text roundText;
	private Text winText;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		fightText = GameObject.Find ("FightText").GetComponent<Text> ();
		roundText = GameObject.Find ("RoundText").GetComponent<Text> ();
		roundText.text = "Round 1";

		winText = GameObject.Find ("WinText").GetComponent<Text> ();
		winText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayRoundFight(int roundNumber) {
		if (audioSource == null) {
			Start ();
		}

		StartCoroutine(PlayRoundFightRoutine(roundNumber));
	}

	IEnumerator PlayRoundFightRoutine(int roundNumber) {
		if (roundNumber == 3) {
			audioSource.PlayOneShot (final);

			StartCoroutine (FadeInText (roundText, .8f));
			roundText.text = "Final round";

			yield return new WaitForSeconds(final.length - 0.2f);

			currentClip = round;
			audioSource.PlayOneShot (currentClip);
		} else {
			audioSource.PlayOneShot (round);

			StartCoroutine (FadeInText (roundText, .8f));
			roundText.text = "Round " + roundNumber;

			yield return new WaitForSeconds(round.length - 0.2f);

			if (roundNumber == 1) {
				currentClip = one;
			}

			if (roundNumber == 2) {
				currentClip = two;
			}

			audioSource.PlayOneShot (currentClip);
		}


		yield return new WaitForSeconds(currentClip.length + 0.5f);

		audioSource.PlayOneShot (fight);

		roundText.text = "";
		StartCoroutine (FadeOutText (fightText, 1.5f));

		Global.controls = true;
	}

	public void PlayWin(int player) {
		StartCoroutine(PlayWinRoutine(player));
	}

	IEnumerator PlayWinRoutine(int player) {
		StartCoroutine (FadeInText (winText, .1f));
		if (player == 1) {
			winText.text = Global.player1File + " wins!";
		}

		if (player == 2) {
			winText.text = Global.player2File + " wins!";
		}

		audioSource.PlayOneShot (you);

		yield return new WaitForSeconds (you.length);

		audioSource.PlayOneShot (win);

		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene (0);
	}

	public IEnumerator FadeOutText(Text text, float time, bool zoomIn = true) {
		text.color = new Color (text.color.r, text.color.g, text.color.b, 1);
		text.gameObject.transform.localScale = new Vector3 (1, 1, 1);

		Vector3 initScale = text.gameObject.transform.localScale;

		while (text.color.a >= 0) {
			text.color = new Color (text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / time));

			if (zoomIn) {
				float scale = (1 / text.color.a);
				text.gameObject.transform.localScale = initScale * scale;
			}

			yield return null;
		}
	}

	public IEnumerator FadeInText(Text text, float time, bool zoomIn = true) {
		text.color = new Color (text.color.r, text.color.g, text.color.b, 0);
		Vector3 initScale = text.gameObject.transform.localScale;

		if (zoomIn) {
			text.gameObject.transform.localScale = Vector3.zero;
		}

		while (text.color.a >= 0) {
			text.color = new Color (text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / time));

			if (zoomIn && text.gameObject.transform.localScale.x <= initScale.x) {
				float scale = (text.color.a) * 2;
				text.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * scale;
			}

			yield return null;
		}
	}
}
