using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FightScript : MonoBehaviour {
	public int time; // duration of a fight in seconds

	private GameObject player1;
	private GameObject player2;
	private PlayerControls player1Script;
	private PlayerControls player2Script;
	private Slider player1LifeSlider;
	private Slider player2LifeSlider;
	private Text timer;
	private Text player1Name;
	private Text player2Name;
	private Text player1Score;
	private Text player2Score;

	private float fightTimer;
	private int secondsRemaining;
	private bool pause = false;
	private int scorePlayer1 = 0;
	private int scorePlayer2 = 0;
	private int scoreToWin = 2;
	private int round;

	private GameObject cameraMain;
	private GameObject background;

	private SpeakerScript speaker;

	void Start () {
		// Default fighters for debugging
		if (Global.player1File == "" || Global.player2File == "") {
			Global.player1File = "Hugo";
			Global.player2File = "MaiLy";
		}

		if (Global.levelFile == "") {
			if (Random.Range (1, 3) == 1) {
				Global.levelFile = "Cats";
			} else {
				Global.levelFile = "CapOmega";
			}

		}

		InitMatch ();
	}

	void Update () {
		if (!pause) {
			fightTimer += Time.deltaTime;
			secondsRemaining = time - (int)fightTimer;
		}

		string displayTime = "";
		displayTime = secondsRemaining.ToString ();
		timer.text = displayTime;

		player1LifeSlider.value = player1Script.life;
		player2LifeSlider.value = player2Script.life;

		player1Score.text = scorePlayer1.ToString();
		player2Score.text = scorePlayer2.ToString();

		Vector3 cameraTarget = (player1.transform.position + player2.transform.position) * 0.5f;
		Vector3 distance = (player2.transform.position - player1.transform.position);
		
		if (Mathf.Abs(distance.x) > 20) {
			cameraMain.GetComponent<Camera> ().orthographicSize = Mathf.Lerp(cameraMain.GetComponent<Camera> ().orthographicSize, 10f, 2f * Time.deltaTime);
		} else {
			cameraMain.GetComponent<Camera> ().orthographicSize = Mathf.Lerp(cameraMain.GetComponent<Camera> ().orthographicSize, 6.9f, 2f * Time.deltaTime);
		}

		cameraMain.transform.position = Vector3.Lerp (cameraMain.transform.position, new Vector3 (cameraTarget.x, cameraMain.transform.position.y, cameraMain.transform.position.z), 5f * Time.deltaTime);
		background.transform.position = new Vector3 (cameraMain.transform.position.x * 0.7f, background.transform.position.y, background.transform.position.z);

		if (player1 != null && player1Script.enabled == true && player1Script.life <= 0 && !pause) {
			scorePlayer2++;

			if (scorePlayer2 < scoreToWin) {
				NewRound ();
			} else {
				Win (2);
			}
		}

		if (player2 != null && player2Script.enabled == true && player2Script.life <= 0 && !pause) {
			scorePlayer1++;

			if (scorePlayer1 < scoreToWin) {
				NewRound ();
			} else {
				Win (1);
			}
		}
	}

	public void InitMatch() {
		// Load level
		Instantiate (Resources.Load ("Prefabs/Levels/" + Global.levelFile), Vector3.zero, Quaternion.Euler(Vector3.zero));

		cameraMain = GameObject.Find ("Main Camera");
		background = GameObject.Find ("LevelBackground");
		speaker = GameObject.Find ("Speaker").GetComponent<SpeakerScript> ();

		timer = GameObject.Find ("Timer").GetComponent<Text> ();
		player1Name = GameObject.Find ("Player1Name").GetComponent<Text> ();
		player2Name = GameObject.Find ("Player2Name").GetComponent<Text> ();
		player1Score = GameObject.Find ("Player1Score").GetComponent<Text> ();
		player2Score = GameObject.Find ("Player2Score").GetComponent<Text> ();

		player1LifeSlider = GameObject.Find ("Player1Life").GetComponent<Slider> ();
		player2LifeSlider = GameObject.Find ("Player2Life").GetComponent<Slider> ();

		round = 0;
		NewRound();
	}

	public void NewRound() {
		if (player1 != null) {
			player1Script.enabled = false;
			player1.transform.Find("Idle").GetComponent<SpriteRenderer> ().enabled = false;
			player1.transform.Find("Punch").GetComponent<SpriteRenderer> ().enabled = false;
			Destroy (player1, .5f);
		}

		if (player2 != null) {
			player2Script.enabled = false;
			player2.transform.Find("Idle").GetComponent<SpriteRenderer> ().enabled = false;
			player2.transform.Find("Punch").GetComponent<SpriteRenderer> ().enabled = false;
			Destroy (player2, .5f);
		}

		Global.controls = false;
		round++;

		player1 = SpawnPlayer (Global.player1File, new Vector3 (-3, -10, 0), Quaternion.Euler (new Vector3 (0, 0, 0)));
		player2 = SpawnPlayer (Global.player2File, new Vector3 (13, -10, 0), Quaternion.Euler (new Vector3 (0, -180, 0)));

		player1.name = Global.player1File;
		player2.name = Global.player2File;

		player1Script = player1.GetComponent<PlayerControls> ();
		player2Script = player2.GetComponent<PlayerControls> ();

		player1Script.SetControls (1);
		player2Script.SetControls (2);

		player1Name.text = player1Script.fighterName != "" ? player1Script.fighterName : player1.name;
		player2Name.text = player2Script.fighterName != "" ? player2Script.fighterName : player2.name;

		player1Script.life = player1Script.initialLife;
		player2Script.life = player2Script.initialLife;

		player1LifeSlider.value = player1Script.life;
		player2LifeSlider.value = player2Script.life;

		/*player1.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
		player2.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
		player1.GetComponent<Rigidbody2D> ().angularVelocity = 0f;
		player2.GetComponent<Rigidbody2D> ().angularVelocity = 0f;

		player1.transform.position = new Vector3 (-8, -10, 0);
		player1.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));

		player2.transform.position = new Vector3 (8, -10, 0);
		player2.transform.rotation = Quaternion.Euler(new Vector3(0,-180,0));

		player1Script.life = player1Script.initialLife;
		player2Script.life = player2Script.initialLife;*/

		fightTimer = 0f;

		speaker.PlayRoundFight (round);
	}

	public GameObject SpawnPlayer(string playerName, Vector3 position, Quaternion rotation) {
		//Grid grid = GameObject.Find ("Grid").GetComponent<Grid> ();
		//RaycastHit2D hit = Physics2D.Raycast (new Vector2(position.x,25), Vector2.down, Mathf.Infinity, LayerMask.NameToLayer("Water"));
		//Debug.Log (grid.WorldToCell(hit.point));
		//Debug.DrawRay (new Vector3(position.x, 25, 0), Vector3.down * 100, Color.white, 100f);
		//Debug.Log (hit.point);
		//Debug.Log (hit.collider.gameObject.name);

		return Instantiate (Resources.Load ("Prefabs/Fighters/" + playerName), position, rotation) as GameObject;
	}

	public void Win(int winner) {
		pause = true;
		Global.controls = false;

		speaker.PlayWin (winner);
	}
}
