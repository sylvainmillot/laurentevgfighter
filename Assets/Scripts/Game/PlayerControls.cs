using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
	protected Rigidbody2D rb;

	public KeyCode leftKey;
	public KeyCode rightKey;
	public string punchKey;
	public string specialKey;
	public string jumpKey;
	public string axis;
	public string punchAxis;
	public int fighterNumber;
	public string fighterName;

	protected float h;
	protected float speed = 600;
	protected float jumpHeight = 1500;

	public int initialLife = 100;
	public int life;

	protected float lastPunchAnim = 0;
	protected float lastPunch = 0;
	protected float lastSpecial = 0;
	protected float lastHit = 0;
	protected bool isHitted = false;
	public bool isGuarding = false;
	protected float punchCoef = 1f;

	protected bool isGrounded;

	protected GameObject idle;
	protected GameObject punch;
	protected PunchScript punchScript;

	protected GameObject enemy;

	protected Vector3 directionToEnemy;

	protected AudioSource audioSource;
	public AudioClip specialSound;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		idle = transform.Find ("Idle").gameObject;
		punch = transform.Find ("Punch").gameObject;
		punchScript = punch.GetComponent<PunchScript> ();
		audioSource = GetComponent<AudioSource> ();

		if (GameObject.FindGameObjectsWithTag ("Player").Length == 2) {
			foreach (GameObject fighter in GameObject.FindGameObjectsWithTag("Player")) {
				if (fighter.GetComponent<PlayerControls> ().fighterNumber != fighterNumber) {
					enemy = fighter;
				}
			}
		}

		life = initialLife;
		isGrounded = false;
	}

	void FixedUpdate () {
		if (enemy != null) {
			// Allways face the other fighter
			if (transform.position.x > enemy.transform.position.x + 5) {
				transform.rotation = Quaternion.Euler (new Vector3 (0, -180, 0));
			}
			if (transform.position.x < enemy.transform.position.x - 5) {
				transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
			}
		}

		if (Global.controls) {
			Controls ();
		}
	}

	void Controls() {
		// Move left or right
		h = Input.GetAxis (axis);
		if (!isHitted) {
			rb.velocity = new Vector2 (h * Time.deltaTime * speed, rb.velocity.y);
		}

		// Jump
		if (Input.GetButtonDown (jumpKey)) {
			if (isGrounded) {
				isGrounded = false;
				rb.velocity = new Vector2 (rb.velocity.x, Time.deltaTime * jumpHeight);
			}
		}

		// Punch
		if (Input.GetButtonDown (punchKey) && (Time.time - lastPunch > .5 || lastPunch == 0)) {
			lastPunch = Time.time;
			lastPunchAnim = Time.time;
			idle.SetActive (false);
			punchScript.Punch ();
		}

		// Special
		if (Input.GetButtonDown (specialKey) && (Time.time - lastSpecial > 5 || lastSpecial == 0)) {
			lastSpecial = Time.time;

			audioSource.PlayOneShot (specialSound);

			float specialX = transform.InverseTransformDirection (Vector3.left).x;
			GameObject hadouken = Instantiate (Resources.Load("Prefabs/Specials/Hadouken"), new Vector3(transform.position.x + specialX * 2, transform.position.y + 18f, -1), transform.rotation) as GameObject;
			hadouken.GetComponent<SpecialScript> ().player = transform.gameObject;
		}

		// Guard
		if (transform.InverseTransformDirection (Vector3.left).x * h > 0) {
			isGuarding = true;
		} else {
			isGuarding = false;
		}

		// Reset after punch
		if (Time.time - lastPunchAnim > 0.2f) {
			lastPunchAnim = 0;
			idle.SetActive (true);
			punchScript.StopPunch ();
		}

		// Reset after hitted
		if (Time.time - lastHit > 0.5f) {
			lastHit = 0;
			isHitted = false;
		}
	}

	public void isPunched(int power) {
		isHitted = true;
		lastHit = Time.time;

		if (gameObject.name == "Laurent") {
			power = power / 2;
		}

		if (!isGuarding) {
			life -= power;
			punchCoef = 1f;
		} else {
			life -= (int)power / 5;
			punchCoef = 0.5f;
		}

		float forceDirection = transform.InverseTransformDirection (Vector3.left).x;
		rb.AddRelativeForce (new Vector2 (Time.deltaTime * speed * 140 * forceDirection * punchCoef, 20000 * Time.deltaTime));
	}

	public void SetControls(int playerPosition = 1) {
		if (playerPosition == 1) {
			leftKey = KeyCode.Q;
			rightKey = KeyCode.D;
			jumpKey = "Jump_P1";
			punchKey = "Fire1_P1";
			specialKey = "Fire2_P1";
			axis = "Player1Horizontal";
			fighterNumber = 1;
		}

		if (playerPosition == 2) {
			leftKey = KeyCode.LeftArrow;
			rightKey = KeyCode.RightArrow;
			jumpKey = "Jump_P2";
			punchKey = "Fire1_P2";
			specialKey = "Fire2_P2";
			axis = "Player2Horizontal";
			fighterNumber = 2;
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.layer == 8 || col.gameObject.tag == "Player") {
			isGrounded = true;
		}
	}
}
