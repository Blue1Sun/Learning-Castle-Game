using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BubbleGame : MonoBehaviour {

	public static int numOfRounds = 5;
	public static int score = 0;

	public GameObject bubblePrefab;
	public GameObject exercise; 
	public int bubbleNum = 6;

	private GameObject window;
	private Text textRounds;
	private int curRound = 0;
	private int answer;

	private int[] xArr = new int[8];
	private float[] posArr = new float[8];

	void Start () {
		score = 0;
		curRound = 0;

		window = GameObject.Find ("Window");
		window.SetActive (false);

		textRounds = GameObject.Find ("Rounds").GetComponent<Text>();
		textRounds.text = "1 / " + numOfRounds;
	}

	void Update () {
		if (!FindObjectOfType<Bubble> ()) {
			if (curRound < numOfRounds) {
				BubbleRand ();
				curRound++;
			}
			else {
				GameEnd ();
			}
			textRounds.text = curRound + " / " + numOfRounds;
		}
	}

	void RandNum () {
		// Create random numbers
		for (int i = 0; i < bubbleNum; i++) {
			int num;
			do {
				num = Random.Range (0, 20); 
			} while (System.Array.IndexOf (xArr, num) > -1); // All answers need to be different
			xArr [i] = num;
			answer = num; // Last random number will be correct answer
		}
	}

	void RandPos () {
		// Create random positions
		for (int i = 0; i < bubbleNum; i++) {
			float position;
			do {
				position = Random.Range (-7.5f, 7.5f);
			} while (System.Array.IndexOf (posArr, position) > -1); // All positions need to be different
			posArr [i] = position;
		}
	}

	// TODO more randomize functions for different exercise

	void BubbleRand () {
		RandNum ();
		RandPos ();

		exercise.GetComponent<Text> ().text = "√" + (answer * answer).ToString (); // TODO different exercise creation 

		for (int i = 0; i < bubbleNum; i++) {
			BubbleSpawn (posArr[i], xArr[i]);
		}
	}

	void BubbleSpawn (float pos, int value) {
		Vector2 pushDown = new Vector2 (0, Random.Range(-10, -40));

		GameObject bubble = Instantiate(bubblePrefab, new Vector3(pos, 7, 0), Quaternion.identity) as GameObject;
		bubble.GetComponent<Rigidbody2D> ().AddForce (pushDown);
		bubble.transform.parent = this.transform;
		bubble.GetComponent<Bubble> ().x = value;

		if (answer == value)
			bubble.GetComponent<Bubble> ().isCorrect = true;
		else
			bubble.tag = "WrongBubble";
	}

	void GameEnd (){		
		string resultMessage;

		window.SetActive (true);

		if (score <= 0)
			resultMessage = "Вам стоит попробовать еще раз. \r\n\r\nВы набрали " + score + " очков.";
		else if (score > 0 && score < numOfRounds)
			resultMessage = "Отличная работа! \r\n\r\nВы набрали " + score + " очков.";
		else 
			resultMessage = "Потрясающий результат, Вы не сделали ни одной ошибки! \r\n\r\nВы набрали " + score + " очков.";

		GameObject.Find("ResultMessage").GetComponent<Text>().text = resultMessage;
	}
}
