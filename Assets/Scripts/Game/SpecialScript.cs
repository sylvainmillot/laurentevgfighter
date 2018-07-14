using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialScript : MonoBehaviour {

	public int power = 35;
	public float speed = 10f;
	public GameObject player;

	void Update () {
		transform.Translate (Vector2.right * speed * Time.deltaTime);

		// Destroy after a few seconds
		Destroy (gameObject, 5f);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player" && col.gameObject.name != player.name) {
			PlayerControls otherPlayerControls = col.gameObject.GetComponent<PlayerControls> ();

			otherPlayerControls.isPunched (power);

			GetComponent<BoxCollider2D> ().enabled = false;
			Destroy (gameObject);
		}
	}
}
