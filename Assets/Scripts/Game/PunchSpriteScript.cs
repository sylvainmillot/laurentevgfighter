using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchSpriteScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AnimationEnd() {
		if (transform.gameObject.activeSelf) {
			transform.gameObject.SetActive (false);
		}

	}
}
