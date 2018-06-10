using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Graphs : MonoBehaviour {

	public int numOfDots;
	public float frequency = 0.2f;

	public GameObject jumper;

	private GameObject window;

	private int curRound;
	private int numOfRounds = 10;
	private int score;
	private bool buttonWerePressed;
	private int[] aArr;
	private float[] kArr;

	void Start() {
		curRound = 0;
		score = 0;

		window = GameObject.Find ("Window");
		window.SetActive (false);

		NextRound ();
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void NextRound(){
		curRound++;
		buttonWerePressed = false;
		GameObject.Find ("Score").GetComponent<Text> ().text = score.ToString ();

		if (curRound <= numOfRounds) {
			Destroy (GameObject.Find ("Jumper"));
			foreach (Transform child in GameObject.Find("Dots").transform)
				Destroy (child.gameObject);
			GameObject newJumper = GameObject.Instantiate (jumper, jumper.transform.position, Quaternion.identity);
			newJumper.name = "Jumper";

			foreach (Transform child in GameObject.Find("Canvas").transform)
				child.tag = "Untagged";

			GameObject.Find ("Rounds").GetComponent<Text> ().text = curRound + " / " + numOfRounds;
			RandomGraphs ();
		} 
		else {
			MiniGame.GameEnd (window, numOfRounds, score, numOfRounds / 2);
		}
	}

	void RandomGraphs(){
		aArr = new int[3];
		kArr = new float[3];

		int a = 0;
		float k  = 0;

		for (int i = 0; i < 3; i++) {
			do {
				a = Random.Range (-3, 3);
				//b = Random.Range (-3, 3);
				k = Random.Range (1, 5) * 0.5f;
			} while ((aArr [0] == a && kArr [0] == k) || (aArr [1] == a && kArr [1] == k));
			aArr [i] = a;
			kArr [i] = k;

			GameObject.Find ((i + 1) + " Graph").GetComponentInChildren<Text> ().text = GraphString (a, k);	
		}
		int correctGraph = Random.Range (1, 4);
		GameObject.Find (correctGraph + " Graph").tag = "CorrectGraph";

		Graph (aArr [correctGraph - 1], kArr [correctGraph - 1], 0.1f, false);
	}

	public void ButtonPressed(int num){
		GameObject button = GameObject.Find (num + " Graph");
		if (button.tag == "CorrectGraph" && !buttonWerePressed)
			score++;
		if (!buttonWerePressed) {
			Graph (aArr [num - 1], kArr [num - 1], 0.05f, true);

			GameObject newJumper = GameObject.Find ("Jumper");
			newJumper.GetComponent<JumperController> ().moving = true;
			newJumper.GetComponent<Rigidbody2D> ().AddForce(Vector2.right * 100);
		}
		buttonWerePressed = true;
	}

	string GraphString(int a, float k){
		string formated = "y = " + k;
		if (curRound < 3)
			formated += "x";
		else if (curRound < 5)
			formated += "x²";
		else if (curRound < 7)
			formated += "x³"; 
		else if (curRound < 9)
			formated += "|x|"; 
		else
			formated += "√x"; 
		if (a < 0)
			formated += " - " + (-a);
		else if (a > 0)
			formated += " + " + a;	
		return formated;
	}
		
	void Graph(int a , float k, float step, bool forPlayer){
		LineRenderer lineRenderer = this.GetComponent<LineRenderer> ();

		float x = -3.5f;
		if (curRound >= 9)
			x = 0;
		int num = 0;
			
		do {
			float y = 0;
			if (curRound < 3)
				y = x * k + a;
			else if (curRound < 5)
				y = x * x * k + a;
			else if (curRound < 7)
				y = x * x * x * k + a;
			else if (curRound < 9)
				y = Mathf.Abs(x) * k + a;
			else
				y = Mathf.Sqrt(x) * k + a;
			
			if (y <= 3.5f && y >= -3.5f){
				if (forPlayer){
					JumperController jumper = GameObject.Find ("Jumper").GetComponent<JumperController>();
					JumperController.Coordinate xy = new JumperController.Coordinate(x,y);
					jumper.playerCoord.Add(xy);
				}
				else {
					num++;
					lineRenderer.numPositions = num;
					lineRenderer.SetPosition (num-1, new Vector3 (x, y, 0));
				}
			}
			x += step; 
		} while (x <= 3.5f);
	}
									
}
