using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleGame : MonoBehaviour {

	public GameObject bubblePrefab;
	public GameObject ex; 
	public int BubbleNum = 3;

	public static int numOfRounds = 5;
	public static int score = 0;
	public static int round = 0;

	private int answer;
	private LevelManager levelManager;
	private GameObject window;
	private Text textRounds;

	private int[] xArr = new int[8];
	private float[] posArr = new float[8];

	// Use this for initialization
	void Start () {
		score = 0;
		round = 0;

		levelManager = FindObjectOfType<LevelManager> ();
		window = GameObject.Find ("Window");
		textRounds = GameObject.Find ("Rounds").GetComponent<Text>();

		textRounds.text = "1 / " + numOfRounds;
		window.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (!FindObjectOfType<Bubble> ()) {
			if (round < numOfRounds && !window.activeSelf) {
				BubbleSpawn ();
				round++;
			}
			else {
				GameEnd ();
			}
			textRounds.text = round + " / " + numOfRounds;
		}
	}

	void Rand(){

		for (int i = 0; i < BubbleNum; i++) {
			int x;
			do {
				x = Random.Range (0, 20); 
			} while (System.Array.IndexOf (xArr, x) > -1);
			xArr [i] = x;
			answer = x;
		}

		for (int i = 0; i < BubbleNum; i++) {
			float pos;
			do {
				pos = Random.Range (-7.5f, 7.5f);
			} while (System.Array.IndexOf (posArr, pos) > -1);
			posArr [i] = pos;
		}
	}

	void BubbleSpawn(){
		Rand ();

		ex.GetComponent<Text> ().text = "√" + (answer * answer).ToString ();

		for (int i = 0; i < BubbleNum; i++) {
			BubbleAct (posArr[i], xArr[i]);
		}
	}

	void BubbleAct(float pos, int value){
		Vector2 frict = new Vector2 (0, Random.Range(-10, -40));

		GameObject bubble = Instantiate(bubblePrefab, new Vector3(pos, 7, 0), Quaternion.identity) as GameObject;
		bubble.GetComponent<Rigidbody2D> ().AddForce (frict);
		bubble.transform.parent = this.transform;
		bubble.GetComponent<Bubble> ().x = value;

		if (answer == value)
			bubble.GetComponent<Bubble> ().isCorrect = true;
		else
			bubble.tag = "WrongBubble";
	}

	void GameEnd (){
		window.SetActive (true);
		string resultMess;
		if (score <= 0)
			resultMess = "Вам стоит попробовать еще раз. \r\n\r\nВы набрали " + score + " очков.";
		else if (score > 0 && score < numOfRounds)
			resultMess = "Отличная работа! \r\n\r\nВы набрали " + score + " очков.";
		else 
			resultMess = "Потрясающий результат, Вы не сделали ни одной ошибки! \r\n\r\nВы набрали " + score + " очков.";

		GameObject.Find("ResultMessage").GetComponent<Text>().text = resultMess;
	}
}
