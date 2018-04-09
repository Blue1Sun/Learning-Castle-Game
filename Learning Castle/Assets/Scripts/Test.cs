using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

using WebSocketSharp;

public class Test : MonoBehaviour {

	[System.Serializable]
	public class QuestionInfo
	{
		public int type; // 1 - oneA, 2 - manyA, 3 - textA
		public int id;
		public string question;
		public string[] answers;

		public QuestionInfo(int type, int id, string question, string[] answers)
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
		public int code = 2; // for socket
		public string[] answers;
		public int[] idOrder;

		public int UserId{
			get	{ return userId; }
			set	{ userId = value; }
		}

		public int Code{
			get	{ return code; }
			set	{ code = value; }
		}

		public string[] Answers{
			get	{ return answers; }
			set	{ answers = value; }
		}

		public int[] IdOrder{
			get	{ return idOrder; }
			set	{ idOrder = value; }
		}

		public PlayerAnswers(int size){
			userId = GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ().Id;
			answers = new string[size];
			idOrder = new int[size];
		}

		public string toString(){
			string answersString = "";

			answersString += userId + " user: ";

			for (int i = 0; i < answers.Length; i++)
				answersString += idOrder [i] + " -- " + answers [i] + "; ";

			return answersString;
		}
	}

	private GameObject toggles;
	private GameObject longAnsw;

	private GameObject[] toggle;

	private int qtype;
	private int curQuestion;

	public QuestionInfo[] questions;
	private PlayerAnswers playerAnswers;

	void Start () {
		QuestionsCreation ();

		setTestTitle ();

		curQuestion = -1;

		toggles = GameObject.Find ("Toggles");
		longAnsw = GameObject.Find ("LongAnswer");

		toggle = new GameObject[4];
		int i = 0;
		foreach (Transform child in toggles.transform) {
			toggle [i] = child.gameObject;
			i++;
		}			

		NextQuestion ();
	}

	private class Message{
		public int code = 3;
		public int topic = Menu.castle;
	}

	//TODO Loading questions from server
	void QuestionsCreation()
	{
		//int numOfQuestions = 7;
		#region SOCKET STUFF

		Message message = new Message ();

		WebSocket socket = new WebSocket("ws://127.0.0.1:16000");
		socket.Connect();

		string jsonmessage = JsonUtility.ToJson (message);
		socket.Send (jsonmessage);

		string testInfo = "";

		socket.OnMessage += (sender, e) => {
			testInfo = "{\r\n    \"Items\": "+ e.Data + "\r\n}";
		};

		System.Threading.Thread.Sleep (500);

		testInfo = testInfo.Replace("sqrt", "√");
		questions = JSONHelper.FromJson<QuestionInfo>(testInfo);

		#endregion

		playerAnswers = new PlayerAnswers(questions.Length); 

		/*questions [0] = new QuestionInfo (1, 823, "Question", new string[] { "a1","a2","a3", "a4" });
		questions [1] = new QuestionInfo (1, 589, "Yes or no?", new string[] { "yes","no" });
		questions [2] = new QuestionInfo (1, 753, "Bear, horse or duck?", new string[] { "bear", "horse", "duck" });

		questions [3] = new QuestionInfo (2, 172, "Questionn", new string[] { "aa1", "aa2", "aa3", "aa4" });
		questions [5] = new QuestionInfo (2, 121, "Yes, YES?", new string[] { "yes","YES" });
		questions [4] = new QuestionInfo (2, 537, "Um hum?", new string[] { "mhm", "hmhm", "mhmmhm" });

		questions [6] = new QuestionInfo (3, 45, "Questionnn", null);*/

		for (int i = 0; i < questions.Length; i++)
			playerAnswers.IdOrder [i] = questions [i].id;

		for (int i = 0; i < questions.Length; i++)
			playerAnswers.Answers [i] = "";
	}

	void setTestTitle(){
		//FILLME: don't forget to fill it!!!
		string[] testTitles = new string[]{ "Квадратный корень", "Квадратные уравнения", "Графики", "Словарный запас", "Артикли" };

		Text title = GameObject.Find ("Title").GetComponent<Text> ();
		if (Menu.castle <= testTitles.Length) {
			title.text = testTitles [Menu.castle - 1];
		} 
		else {
			title.text = "Нет названия";
			Debug.LogWarning ("No title for " +  Menu.castle + " test");
		}		
	}

	public void PrevQuestion(){
		// Saving player answer
		if (qtype == 3)
			playerAnswers.Answers [curQuestion] = GameObject.Find ("PlayerInput").GetComponent<Text> ().text;
		else
			playerAnswers.Answers [curQuestion] = AnswerReadingTog ();
		
		curQuestion--;

		qtype = questions [curQuestion].type;
		ChangeLayout (qtype);

		Debug.Log (playerAnswers.toString());
	}

	// Checking type of next question
	public void NextQuestion() {		
		// Saving player answer if it's not test start
		if (qtype == 3 && curQuestion != -1)
			playerAnswers.Answers [curQuestion] = GameObject.Find ("PlayerInput").GetComponent<Text> ().text;
		else if (curQuestion != -1)
			playerAnswers.Answers [curQuestion] = AnswerReadingTog ();
		
		curQuestion++;

		if (curQuestion + 1 > questions.Length){
			FinishTest ();
		}
		else {
			qtype = questions [curQuestion].type;
			ChangeLayout (qtype);
		}

		Debug.Log (playerAnswers.toString());
	}

	void FinishTest(){
		GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ().CompletedTests [Menu.castle - 1] = true;

		#region SOCKET STUFF

			WebSocket socket = new WebSocket("ws://127.0.0.1:16000");
			socket.Connect();
			string jsonmessage = JsonUtility.ToJson(playerAnswers);
			socket.Send(jsonmessage);
			
		#endregion

		SceneManager.LoadScene ("Menu");
	}

	string AnswerReadingTog()
	{
		Toggle a1 = toggle[0].GetComponent<Toggle>();
		Toggle a2 = toggle[1].GetComponent<Toggle>();
		Toggle a3 = toggle[2].GetComponent<Toggle>();
		Toggle a4 = toggle[3].GetComponent<Toggle>();

		string s = "";

		if (a1.isOn)
			s += "1";
		if (a2.isOn)
			s += "2";
		if (a3.isOn && a3.gameObject.activeSelf)
			s += "3";
		if (a4.isOn  && a3.gameObject.activeSelf)
			s += "4";
		
		return s;
	}

	void ChangeLayout(int qtype)
	{
		// Required initialization
		Toggle answer1 = toggle[0].GetComponent<Toggle>();
		Toggle answer2 = toggle[1].GetComponent<Toggle>();
		Toggle answer3 = toggle[2].GetComponent<Toggle>();
		Toggle answer4 = toggle[3].GetComponent<Toggle>();


		GameObject.Find ("QNum").GetComponent<Text> ().text = "Вопрос №" + (curQuestion + 1) + " из " + questions.Length;
		GameObject.Find ("Question").GetComponent<Text> ().text = questions [curQuestion].question;

		if (qtype == 2 || qtype == 1) {
			toggles.SetActive (true);
			longAnsw.SetActive (false);

			GameObject.Find ("1Answer").GetComponentInChildren<Text> ().text = questions [curQuestion].answers [0];
			GameObject.Find ("2Answer").GetComponentInChildren<Text> ().text = questions [curQuestion].answers [1];

			GameObject answer3Obj = answer3.gameObject;
			GameObject answer4Obj = answer4.gameObject;

			answer3Obj.SetActive (false);
			answer4Obj.SetActive (false);

			if (questions [curQuestion].answers.Length > 2) {		
				answer3Obj.SetActive (true);
				GameObject.Find ("3Answer").GetComponentInChildren<Text> ().text = questions [curQuestion].answers [2];
			} 

			if (questions [curQuestion].answers.Length > 3) {
				answer4Obj.SetActive (true);
				GameObject.Find ("4Answer").GetComponentInChildren<Text> ().text = questions [curQuestion].answers [3];
			}
		}
		else {
			toggles.SetActive(false);
			longAnsw.SetActive (true);

			GameObject.Find ("InputAnswer").GetComponent<InputField> ().text = playerAnswers.Answers [curQuestion];
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
		if ((curQuestion + 1) == questions.Length) {
			bNext.GetComponentInChildren<Text> ().text = "Завершить";
			bNext.GetComponentInChildren<Text> ().fontStyle = FontStyle.Bold;
		} else {
			bNext.GetComponentInChildren<Text> ().text = "Далее";
			bNext.GetComponentInChildren<Text> ().fontStyle = FontStyle.Normal;
		}
	}
		
	void TogglesSwitch(Toggle answer1, Toggle answer2, Toggle answer3, Toggle answer4)
	{
		ToggleGroup toggleGroup = toggles.GetComponent<ToggleGroup> ();

		toggleGroup.allowSwitchOff = true;
		answer1.isOn = answer2.isOn = answer3.isOn = answer4.isOn = false;
		toggleGroup.allowSwitchOff = false;

		if (playerAnswers.Answers [curQuestion].Contains ("1"))
			answer1.isOn = true;
		if (playerAnswers.Answers [curQuestion].Contains ("2"))
			answer2.isOn = true;
		if (playerAnswers.Answers [curQuestion].Contains ("3"))
			answer3.isOn = true;
		if (playerAnswers.Answers [curQuestion].Contains ("4"))
			answer4.isOn = true;
	}
}
