using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchColliderScript : MonoBehaviour {

	public Vector3 initialPosition;

	protected PunchScript punchScript;
	protected GameObject player;
	private GameObject punchSprite;

	// Use this for initialization
	void Start () {
		initialPosition = transform.localPosition;
		punchScript = transform.parent.GetComponent<PunchScript> ();
		player = transform.parent.parent.gameObject;
		if (player.transform.Find ("PunchSprite") != null) {
			punchSprite = player.transform.Find ("PunchSprite").gameObject;
		}
	}
	
	void OnTriggerStay2D(Collider2D col) {
		if (col.gameObject.tag == "Player" && col.gameObject.name != punchScript.player.name && punchScript.isPunching) {
			PlayerControls otherPlayerControls = col.gameObject.GetComponent<PlayerControls> ();

			if(punchSprite != null) { 
				if (otherPlayerControls.isGuarding) {
					punchSprite.GetComponent<SpriteRenderer> ().color = new Color (0f, 0f, 0f, .5f);
				} else {
					punchSprite.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
				}

				punchSprite.SetActive (true);
			}

			punchScript.audioSource.Play ();
			otherPlayerControls.isPunched (punchScript.power);
			punchScript.isPunching = false;
		}
	}
}
