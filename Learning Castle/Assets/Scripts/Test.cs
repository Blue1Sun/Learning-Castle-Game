using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using WebSocketSharp;

public class Test : MonoBehaviour {

	public class Question
	{
		public int type; // 1 - oneA, 2 - manyA, 3 - textA
		public int id;
		public string question;
		public string[] answers;

		public Question(int type, int id, string question, string[] answers)
		{
			this.type = type;
			this.id = id;
			this.question = question;
			this.answers = answers;
		}
	};

	public class PlayerAnswers
	{
		public int userId;
		public int code = 2;
		public string[] answers;
		public int[] idOrder;

		public PlayerAnswers(int size){
			userId = GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ().id;
			answers = new string[size];
			idOrder = new int[size];
		}
	}

	private GameObject toggles;
	private GameObject longAnsw;

	private int qtype;
	private int numOfQuestions = 3;
	private int curQuestion;

	private Question[] questions;
	private PlayerAnswers playerAnswers;

	void Start () {
		QuestionsCreation ();
		if (Menu.castle == 1)
			GameObject.Find ("Title").GetComponent<Text> ().text = "Квадратный корень";	
		else if (Menu.castle == 2)
			GameObject.Find ("Title").GetComponent<Text> ().text = "Квадратные уравнения";	
		else if (Menu.castle == 3)
			GameObject.Find ("Title").GetComponent<Text> ().text = "Графики";
		else if (Menu.castle == 4)
			GameObject.Find ("Title").GetComponent<Text> ().text = "Словарный запас";
		//TODO: don't forget to fill it!!!

		curQuestion = -1;

		toggles = GameObject.Find ("Toggles");
		longAnsw = GameObject.Find ("LongAnswer");

		NextQuestion ();
	}

	//TODO Loading questions from server
	void QuestionsCreation()
	{
		questions = new Question[numOfQuestions]; 
		playerAnswers = new PlayerAnswers(numOfQuestions); 
		//playerAnswers = new string[numOfQuestions];
		//idOrder = new int[numOfQuestions];

		questions [0] = new Question (1, 823, "Question", new string[] { "a1","a2","a3", "a4" });
		questions [1] = new Question (2, 172, "Questionn", new string[] { "aa1", "aa2", "aa3", "aa4" });
		questions [2] = new Question (3, 45, "Questionnn", null);

		for (int i = 0; i < questions.Length; i++)
			playerAnswers.idOrder [i] = questions [i].id;

		for (int i = 0; i < questions.Length; i++)
			playerAnswers.answers [i] = "";
	}

	public void PrevQuestion(){
		// Saving player answer
		if (qtype == 3)
			playerAnswers.answers [curQuestion] = GameObject.Find ("PlayerInput").GetComponent<Text> ().text;
		else
			playerAnswers.answers [curQuestion] = AnswerReadingTog ();
		
		curQuestion--;

		qtype = questions [curQuestion].type;
		ChangeLayout (qtype);

		Debug.Log (playerAnswers.idOrder[0] + " = " + playerAnswers.answers [0] + ", " + playerAnswers.idOrder[1] + " = " + playerAnswers.answers [1] + ", " + playerAnswers.idOrder[2] + " = " + playerAnswers.answers [2]);
	}

	// Checking type of next question
	public void NextQuestion() {		
		// Saving player answer if it's not test start
		if (qtype == 3 && curQuestion != -1)
			playerAnswers.answers [curQuestion] = GameObject.Find ("PlayerInput").GetComponent<Text> ().text;
		else if (curQuestion != -1)
			playerAnswers.answers [curQuestion] = AnswerReadingTog ();
		
		curQuestion++;

		if (curQuestion + 1 > numOfQuestions){
			// Finishing test
			GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ().completedTests [Menu.castle - 1] = true;

			#region SOCKET STUFF
			/*
			WebSocket socket = new WebSocket("ws://127.0.0.1:16000");
			socket.Connect();
			string jsonmessage = JsonUtility.ToJson(playerAnswers);
			socket.Send(jsonmessage);
			*/
			#endregion

			SceneManager.LoadScene ("Menu"); // TODO sending answers to server
		}
		else {
			qtype = questions [curQuestion].type;
			ChangeLayout (qtype);
		}

		Debug.Log (playerAnswers.idOrder[0] + " = " + playerAnswers.answers [0] + ", " + playerAnswers.idOrder[1] + " = " + playerAnswers.answers [1] + ", " + playerAnswers.idOrder[2] + " = " + playerAnswers.answers [2]);
	}

	string AnswerReadingTog()
	{
		Toggle a1 = null;
		Toggle a2 = null;
		Toggle a3 = null;
		Toggle a4 = null;

		FindToggles (ref a1, ref a2, ref a3, ref a4);

		string s = "";

		if (a1.isOn)
			s += "1";
		if (a2.isOn)
			s += "2";
		if (a3.isOn)
			s += "3";
		if (a4.isOn)
			s += "4";
		
		return s;
	}

	void ChangeLayout(int qtype)
	{
		// Required initialization
		Toggle answer1 = null;
		Toggle answer2 = null;
		Toggle answer3 = null;
		Toggle answer4 = null;

		GameObject.Find ("QNum").GetComponent<Text> ().text = "Вопрос №" + (curQuestion + 1) + " из " + numOfQuestions;
		GameObject.Find ("Question").GetComponent<Text> ().text = questions [curQuestion].question;

		if (qtype == 2 || qtype == 1) {
			toggles.SetActive (true);
			longAnsw.SetActive (false);

			FindToggles (ref answer1, ref answer2, ref answer3, ref answer4);
			GameObject.Find ("1Answer").GetComponentInChildren<Text> ().text = questions [curQuestion].answers [0];
			GameObject.Find ("2Answer").GetComponentInChildren<Text> ().text = questions [curQuestion].answers [1];
			GameObject.Find ("3Answer").GetComponentInChildren<Text> ().text = questions [curQuestion].answers [2];
			GameObject.Find ("4Answer").GetComponentInChildren<Text> ().text = questions [curQuestion].answers [3];
		} else {
			toggles.SetActive(false);
			longAnsw.SetActive (true);

			GameObject.Find ("PlayerInput").GetComponent<Text> ().text = playerAnswers.answers [curQuestion];
		}

		if (qtype == 2) {
			answer1.group = answer2.group = answer3.group = answer4.group = null;
			TogglesSwitch (answer1, answer2, answer3, answer4);			
		} 
		else if (qtype == 1) {
			ToggleGroup toggleGroup = GameObject.Find ("Toggles").GetComponent<ToggleGroup> ();

			answer1.group = answer2.group = answer3.group = answer4.group = toggleGroup;
			TogglesSwitch (answer1, answer2, answer3, answer4);
		} 

		GameObject bBack = GameObject.Find ("Back");
		GameObject bNext = GameObject.Find ("Next");

		// First question
		if (curQuestion == 0) {			
			bBack.GetComponent<Button> ().interactable = false;
			bBack.GetComponentInChildren<Text> ().color = new Color (0.2f, 0.2f, 0.2f, 0.5f);
		} else {
			bBack.GetComponent<Button> ().interactable = true;
			bBack.GetComponentInChildren<Text> ().color = new Color (0.2f, 0.2f, 0.2f, 1f);
		}

		// Last question
		if ((curQuestion + 1) == numOfQuestions) {
			bNext.GetComponentInChildren<Text> ().text = "Завершить";
			bNext.GetComponentInChildren<Text> ().fontStyle = FontStyle.Bold;
		} else {
			bNext.GetComponentInChildren<Text> ().text = "Далее";
			bNext.GetComponentInChildren<Text> ().fontStyle = FontStyle.Normal;
		}
	}

	void FindToggles(ref Toggle a1, ref Toggle a2, ref Toggle a3, ref Toggle a4)
	{
		a1 = GameObject.Find ("1Answer").GetComponent<Toggle> ();
		a2 = GameObject.Find ("2Answer").GetComponent<Toggle> ();
		a3 = GameObject.Find ("3Answer").GetComponent<Toggle> ();
		a4 = GameObject.Find ("4Answer").GetComponent<Toggle> ();
	}

	void TogglesSwitch(Toggle answer1, Toggle answer2, Toggle answer3, Toggle answer4)
	{
		answer1.isOn = answer2.isOn = answer3.isOn = answer4.isOn = false;
		if (playerAnswers.answers [curQuestion].Contains ("1"))
			answer1.isOn = true;
		if (playerAnswers.answers [curQuestion].Contains ("2"))
			answer2.isOn = true;
		if (playerAnswers.answers [curQuestion].Contains ("3"))
			answer3.isOn = true;
		if (playerAnswers.answers [curQuestion].Contains ("4"))
			answer4.isOn = true;
	}
}
