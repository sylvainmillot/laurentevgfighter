using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchScript : MonoBehaviour {
	
	public GameObject player;
	protected SpriteRenderer sprite;
	public bool isPunching;
	public int power;
	public AudioSource audioSource;
	protected PunchColliderScript punchCollider;

	void Start () {
		player = transform.parent.gameObject;
		sprite = GetComponent<SpriteRenderer> ();
		audioSource = GetComponent<AudioSource> ();
		punchCollider = transform.Find ("PunchCollider").GetComponent<PunchColliderScript> ();

		isPunching = false;
		sprite.enabled = false;
		power = 20;
	}

	public void Punch() {
		isPunching = true;
		sprite.enabled = true;

		// We move a little bit the collider position to activate the OnCollisionStay
		punchCollider.transform.localPosition = punchCollider.initialPosition + new Vector3(0.1f,0,0);
	}

	public void StopPunch() {
		isPunching = false;
		if (sprite != null && punchCollider != null) {
			sprite.enabled = false;

			// Move the collider to its initial position
			punchCollider.transform.localPosition = punchCollider.initialPosition;
		}
	}
}
