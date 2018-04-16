using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using WebSocketSharp;

public class BubbleGame : MonoBehaviour {

	[SerializeField]
	private GameObject bubblePrefab = null;
	[SerializeField]
	private int bubbleNum = 6;

	private GameObject window;
	private Text textRounds;
	private Text exercise;

	private int curRound = 0;
	private int minRand, maxRand;
	private int numOfRounds = 5;
	private int score = 0;

	public int Score {
		get	{ return score;	}
		set	{ score = value; }
	}

	private int[] xArr = new int[8];
	private float[] posArr = new float[8];
	private string[] xxArr = new string[8];

	void Start () {
		if (Menu.castle == 1)
			numOfRounds = 10;
		else if (Menu.castle == 2)
			numOfRounds = 5;

		score = 0;
		curRound = 0;

		minRand = 0;
		maxRand = 10;

		window = GameObject.Find ("Window");
		window.SetActive (false);

		textRounds = GameObject.Find ("Rounds").GetComponent<Text>();
		textRounds.text = "1 / " + numOfRounds;

		exercise = GameObject.Find ("Exercise").GetComponent<Text> ();
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

	void RandSqrt () {
		if(curRound != 0){
			minRand += 2;
			maxRand += 2;
		}
		int answer = 0;

		// Create random numbers
		for (int i = 0; i < bubbleNum; i++) {
			int num;
			do {
				num = Random.Range (minRand, maxRand); 
			} while (System.Array.IndexOf (xArr, num) > -1); // All answers need to be different
			xArr [i] = num;
			answer = num;
		}

		exercise.text = "√" + (answer * answer).ToString (); 
	}

	void RandQuadEq () {
		// Create random numbers
		int a, b, c, x1, x2;

		a = Random.Range (1, 3); 
		x1 = Random.Range (-10, 10); 
		x2 = Random.Range (-10, 10); 
		b = -((x1 + x2) * a);
		c = x1 * x2 * a;

		xxArr [bubbleNum - 1] = "x1 = " + x1 + "; x2 = " + x2;

		for (int i = 0; i < bubbleNum - 1; i++) {
			int fakeX1, fakeX2;
			do {
				fakeX1 = Random.Range (x1 - 2, x1 + 2); 
				fakeX2 = Random.Range (x2 - 2, x2 + 2);
			} while (System.Array.IndexOf (xxArr, "x1 = " + fakeX1 + "; x2 = " + fakeX2) != -1 ||
			         System.Array.IndexOf (xxArr, "x1 = " + fakeX2 + "; x2 = " + fakeX1) != -1); // All answers need to be different

			xxArr [i] = "x1 = " + fakeX1 + "; x2 = " + fakeX2;
		}

		exercise.text = formatEx (a, b, c); 
	}

	string formatEx(int a, int b, int c){
		string formated = "";

		if (a == 1)
			formated += "x² ";
		else
			formated += a + "x² ";

		if (b == -1)
			formated += "- x ";
		else if (b < 0)
			formated += "- " + -b + "x ";
		else if (b == 1)
			formated += "+ x ";
		else if (b == 0)
			formated += "";
		else
			formated += "+ " + b + "x ";
		
		if (c < 0)
			formated += "- " + -c + " = 0";
		else if (c == 0)
			formated += "= 0";
		else
			formated += "+ " + c + " = 0";
		return formated;
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

	void BubbleRand () {
		if (Menu.castle == 1)
			RandSqrt ();
		if (Menu.castle == 2)
			RandQuadEq ();
		RandPos ();

		for (int i = 0; i < bubbleNum; i++) {
			bool isCorrect = false;
			if (i == bubbleNum - 1)
				isCorrect = true; // Last bubble is correct
			if (Menu.castle == 1)
				BubbleSpawn (posArr[i], xArr[i], isCorrect);
			if (Menu.castle == 2)
				BubbleSpawn (posArr[i], xxArr[i], isCorrect);
		}
	}

	void BubbleSpawn (float pos, int value, bool isCorrect) {
		GameObject bubble = BubbleInst (pos, isCorrect);
		bubble.GetComponent<Bubble> ().X = value;

		Vector2 pushDown = new Vector2 (0, Random.Range(-10, -40));
		bubble.GetComponent<Rigidbody2D> ().AddForce (pushDown);
	}

	void BubbleSpawn (float pos, string value, bool isCorrect){
		GameObject bubble = BubbleInst (pos, isCorrect);
		bubble.GetComponent<Bubble> ().X1X2 = value;

		bubble.GetComponent<Rigidbody2D> ().velocity = new Vector2(0, -0.35f);
		bubble.GetComponent<SpriteRenderer> ().color = Color.HSVToRGB (Random.Range (0f, 1f), 0.4f, 1);
	}

	GameObject BubbleInst (float x, bool isCorrect){
		GameObject bubble = Instantiate(bubblePrefab, new Vector2(x, 7), Quaternion.identity) as GameObject;
		bubble.transform.parent = this.transform;

		if (isCorrect)
			bubble.GetComponent<Bubble> ().IsCorrect = true;
		else
			bubble.tag = "WrongBubble";

		return bubble;
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

		PlayerData playerInfo = GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ();

		if(score > playerInfo.MinigameRecord [Menu.castle - 1]){
			#region SOCKET STUFF
			if(WebSockets.isSocket){
				UserRecord userRecord = new UserRecord(playerInfo.Id, Menu.castle, score, numOfRounds); 

				WebSocket socket = new WebSocket("ws://127.0.0.1:16000");
				socket.Connect();

				string jsonmessage = JsonUtility.ToJson (userRecord);
				socket.Send (jsonmessage);

				socket.Close();
			}
			#endregion

			playerInfo.MinigameRecord [Menu.castle - 1] = score;
		}
			
		GameObject.Find("ResultMessage").GetComponent<Text>().text = resultMessage;
	}
}
