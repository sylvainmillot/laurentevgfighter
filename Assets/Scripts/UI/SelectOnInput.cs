using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SelectOnInput : MonoBehaviour {

	public EventSystem eventSystem;
	public GameObject selectedObject;

	private bool buttonSelected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetAxisRaw ("Vertical") != 0 || Input.GetAxisRaw ("Horizontal") != 0) && !buttonSelected) {
			eventSystem.SetSelectedGameObject (selectedObject);
			buttonSelected = true;
		}
	}

	void OnDisable() {
		buttonSelected = false;
	}
}
