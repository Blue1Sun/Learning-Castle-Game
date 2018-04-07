using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Graphs : MonoBehaviour {

	public int numOfDots;
	public float frequency = 0.2f;

	//public GraphType graphType;
	//public enum GraphType {Parabola, Linear, Sqrt};
	public GameObject jumper;

	private float[,] playerArr;



	private int curRound;
	private int numOfRounds = 10;
	private int score;
	private bool buttonWerePressed;
	private int[] aArr;
	private float[] kArr;

	void Start() {
		curRound = 0;
		score = 0;

		playerArr = new float[,] { { -2, 4 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };
		NextRound ();
	}

	// Update is called once per frame
	void Update () {
		//if (graphType == GraphType.Parabola)
			//ParabolaGraph (0, 1);
		//if (graphType == GraphType.Linear)
			//LinearGraph ();
		//if (graphType == GraphType.Sqrt)
			//SqrtGraph ();		
	}

	public void NextRound(){
		curRound++;
		buttonWerePressed = false;

		if (curRound <= numOfRounds) {
			Destroy (GameObject.Find ("Jumper"));
			foreach (Transform child in GameObject.Find("Dots").transform)
				Destroy (child.gameObject);
			GameObject newJumper = GameObject.Instantiate (jumper, jumper.transform.position, Quaternion.identity);
			newJumper.name = "Jumper";

			foreach (Transform child in GameObject.Find("Canvas").transform)
				child.tag = "Untagged";

			GameObject.Find ("Rounds").GetComponent<Text> ().text = curRound + " / " + numOfRounds;
			GameObject.Find ("Score").GetComponent<Text> ().text = score.ToString ();
			RandomGraphs ();
		} 
		else {
			GameEnd ();
		}
	}

	void GameEnd(){
		Debug.Log ("Game end");
		/*string resultMessage;

		window.SetActive (true);

		if (score <= numOfRounds / 2)
			resultMessage = "Вам стоит попробовать еще раз. \r\n\r\nВы набрали " + score + " очков.";
		else if (score > numOfRounds / 2 && score < numOfRounds)
			resultMessage = "Отличная работа! \r\n\r\nВы набрали " + score + " очков.";
		else 
			resultMessage = "Потрясающий результат, Вы не сделали ни одной ошибки! \r\n\r\nВы набрали " + score + " очков.";
		
		PlayerData playerData = GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ();
		if (score > playerData.MinigameRecord [Menu.castle - 1])
			playerData.MinigameRecord [Menu.castle - 1] = score;
		
		GameObject.Find("ResultMessage").GetComponent<Text>().text = resultMessage;
		*/
	}

	void RandomGraphs(){
		aArr = new int[3];
		kArr = new float[3];

		int a = 0;
		float k  = 0;

		for (int i = 0; i < 3; i++) {
			do {
				a = Random.Range (-3, 3);
				k = Random.Range (1, 5) * 0.5f;
			} while ((aArr [0] == a && kArr [0] == k) || (aArr [1] == a && kArr [1] == k));
			aArr [i] = a;
			kArr [i] = k;

			if (a < 0)
				GameObject.Find ((i + 1) + " Graph").GetComponentInChildren<Text> ().text = "y = " + k + "x² - " + (-a);
			else if (a == 0)
				GameObject.Find ((i + 1) + " Graph").GetComponentInChildren<Text> ().text = "y = " + k + "x²";
			else if (a > 0)
				GameObject.Find ((i + 1) + " Graph").GetComponentInChildren<Text> ().text = "y = " + k + "x² + " + a;					
		}
		int correctGraph = Random.Range (1, 4);
		GameObject.Find (correctGraph + " Graph").tag = "CorrectGraph";

		ParabolaGraph (aArr [correctGraph - 1], kArr [correctGraph - 1]);
	}

	public float MinXSqrt(){
		return playerArr [0, 0];
	}

	public void ButtonPressed(int num){
		GameObject button = GameObject.Find (num + " Graph");
		if (button.tag == "CorrectGraph" && !buttonWerePressed)
			score++;
		if (!buttonWerePressed) {
			ParabolaGraphPlayer (aArr [num - 1], kArr [num - 1]);

			GameObject.Find ("Jumper").GetComponent<JumperController> ().moving = true;
			GameObject.Find ("Jumper").GetComponent<Transform> ().position += Vector3.right * 2 * Time.deltaTime;
		}

		buttonWerePressed = true;
	}

	void ParabolaGraphPlayer(int a , float k){
		float x = -3.5f;

		do {
			float y = x * x * k + a;
			if (y <= 3.5f && y >= -3.5f){

				JumperController jumper = GameObject.Find ("Jumper").GetComponent<JumperController>();
				JumperController.Coordinate xy = new JumperController.Coordinate(x,y);
				jumper.playerCoord.Add(xy);

				//lineRenderer.SetPosition (num-1, new Vector3 (x, y, 0));
			}
			x += 0.05f;
		} while (x <= 3.5f);
	}

	void ParabolaGraph(int a , float k){
		LineRenderer lineRenderer = this.GetComponent<LineRenderer> ();

		//lineRenderer.numPositions = numOfDots;
		//float x = (-numOfDots / 2 + 0.5f) * frequency;
		float x = -3.5f;
		int num = 0;
			
		//for (int index = 0; index < numOfDots; index++) {
		do {
			float y = x * x * k + a;
			if (y <= 3.5f && y >= -3.5f){
				num++;
				lineRenderer.numPositions = num;
				lineRenderer.SetPosition (num-1, new Vector3 (x, y, 0));
			}
			x += 0.1f;
		} while (x <= 3.5f);
		//}
	}

	void LinearGraph(){
		LineRenderer lineRenderer = this.GetComponent<LineRenderer> ();
		lineRenderer.numPositions = numOfDots;
		int index = 0;
		for (float x = -(numOfDots / 2) * frequency; x < (numOfDots / 2) * frequency && index < numOfDots; x += frequency) {
			float y = x;
			lineRenderer.SetPosition (index, new Vector3 (x, y, 0));
			index++;
		}
	}

	void SqrtGraph(){
		LineRenderer lineRenderer = this.GetComponent<LineRenderer> ();

		lineRenderer.numPositions = numOfDots;
		int index = 0;
		for (float x = 0; x < numOfDots * frequency && index < numOfDots; x += frequency) {
			float y = Mathf.Sqrt(x);
			lineRenderer.SetPosition (index, new Vector3 (x, y, 0));
			index++;
		}
	}
					
}
