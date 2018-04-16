using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using WebSocketSharp;

public class ArticleGame : MonoBehaviour {

	public GameObject walls;

	private int hearts;
	private int curRound;
	private int numOfRounds = 10;
	private int score;

	private string cGen;
	private GameObject window;

	private class Task{
		private string sentence = "";
		private int article; // 1 = the, 2 = a, 3 = none
		private bool isUsed = false;

		public int Article{
			get	{ return article; }
		}

		public string Sentence{
			get	{ return sentence; }
		}

		public bool IsUsed {
			get	{ return isUsed; }
			set	{ isUsed = value; }
		}

		public Task(string sentence, string article){
			this.sentence = sentence;
			if (article.Equals("the"))
				this.article = 1;
			if (article.Equals("a"))
				this.article = 2;
			if (article.Equals(""))
				this.article = 3;
		}
	}

	private Task[] tasks;

	// Use this for initialization
	void Start () {
		score = 0;
		curRound = 0;
		hearts = 3;

		string gender = GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ().Gender;
		cGen = "";
		if (gender.Equals("f"))
			cGen = "W";
		
		GetComponent<Animator>().Play ("ArticleRun" + cGen);

		window = GameObject.Find ("Window");
		window.SetActive (false);

		CreatingTasks ();
		NextRound ();
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow)) && GetComponent<Transform> ().position.x >= 0) {
			GetComponent<Transform> ().position += Vector3.left * 6;
		}
		if ((Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)) && GetComponent<Transform> ().position.x <= 0) {
			GetComponent<Transform> ().position += Vector3.right * 6;
		}
	}
		

	void NextRound(){
		curRound++;
		if (curRound > numOfRounds || hearts == 0)
			GameEnd ();
		else {
			GameObject wallObj = Instantiate (walls, walls.transform.position, Quaternion.identity) as GameObject;
			wallObj.name = "Walls";
			ChangeLayout ();
		}
	}

	void ChangeLayout(){
		int randTask = -1;
		do{
			randTask = Random.Range(0, tasks.Length);
		} while (tasks[randTask].IsUsed);
		tasks[randTask].IsUsed = true;

		GameObject.Find ("Sentence").GetComponent<Text> ().text = tasks [randTask].Sentence;
		GameObject.Find ("Rounds").GetComponent<Text> ().text = curRound + "/" + numOfRounds;

		foreach (Transform child in GameObject.Find ("Walls").GetComponent<Transform> ()) {
			if (child.name == ("Wall" + tasks [randTask].Article))
				child.tag = "CorrectWall";
			else
				child.tag = "Untagged";
		}
	}

	void CreatingTasks(){
		tasks = new Task[20];
		tasks [0] = new Task ("Can anyone give me ___ hand please because I have just fallen over?", "a");
		tasks [1] = new Task ("She always said that when she grew up she wanted to be ___ doctor.", "a");
		tasks [2] = new Task ("I have left my book in ___ kitchen and I would like you to get it for me.", "the");
		tasks [3] = new Task ("Russian people like ___ tea.", "");
		tasks [4] = new Task ("Bring ___ milk from the kitchen.", "the");

		tasks [5] = new Task ("___ Browns have left London.", "the");
		tasks [6] = new Task ("Great Britain consists of ___ three parts.", "");
		tasks [7] = new Task ("It is evident that ___ people want peace.", "");
		tasks [8] = new Task ("The acting was poor, but we enjoyed ___ music.", "the");
		tasks [9] = new Task ("___ Tudors had never possessed a standing army or a police force.", "the");

		tasks [10] = new Task ("I want to go to the cinema to see a film about ___ France and the French.", "");
		tasks [11] = new Task ("Is there ___ school in the street?", "a");
		tasks [12] = new Task ("My father is ___ engineer.", "a");
		tasks [13] = new Task ("There are ___ books and toys on the floor.", "");
		tasks [14] = new Task ("Is there ___ letter for me?", "a");

		tasks [15] = new Task ("The Queen of Great Britain is not ___ absolute.", "");
		tasks [16] = new Task ("Pushkin, the great Russian poet, was born in ___ 1799.", "");
		tasks [17] = new Task ("Could you give me ___ sheet of paper?", "a");
		tasks [18] = new Task ("Tell him ___ truth.", "the");
		tasks [19] = new Task ("This is ___ best wine I have ever drunk.", "the");
	}

	void GameEnd (){		
		string resultMessage;

		window.SetActive (true);

		if (score <= numOfRounds / 2)
			resultMessage = "Вам стоит попробовать еще раз. \r\n\r\nВы набрали " + score + " очков.";
		else if (score > numOfRounds / 2 && score < numOfRounds)
			resultMessage = "Отличная работа! \r\n\r\nВы набрали " + score + " очков.";
		else 
			resultMessage = "Потрясающий результат, Вы не сделали ни одной ошибки! \r\n\r\nВы набрали " + score + " очков.";

		PlayerData playerData = GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ();

		if(score > playerData.MinigameRecord [Menu.castle - 1]){
			#region SOCKET STUFF
			if(WebSockets.isSocket){
				UserRecord userRecord = new UserRecord(playerData.Id, Menu.castle, score, numOfRounds); 

				WebSocket socket = new WebSocket("ws://127.0.0.1:16000");
				socket.Connect();

				string jsonmessage = JsonUtility.ToJson (userRecord);
				socket.Send (jsonmessage);

				socket.Close();
			}
			#endregion

			playerData.MinigameRecord [Menu.castle - 1] = score;
		}

		GameObject.Find("ResultMessage").GetComponent<Text>().text = resultMessage;
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "CorrectWall") {
			score++;
			GameObject.Find ("Score").GetComponent<Text> ().text = score.ToString ();
		} 
		else{
			GameObject.FindGameObjectWithTag ("CorrectWall").GetComponent<Animator> ().Play ("WallFading");
			GetComponent<Animator> ().Play ("ArticleDamage" + cGen);
			hearts--;
			GameObject.Find ("Heart " + (hearts + 1)).GetComponent<RawImage> ().color = new Color (1, 1, 1, 0.5f);
		}
		Destroy (collider.gameObject);

		GameObject oldWalls = GameObject.Find ("Walls");
		if (oldWalls) {
			oldWalls.name = "OldWalls";
			foreach (Transform child in oldWalls.GetComponent<Transform> ())
				child.gameObject.layer = 9;
		}
		NextRound ();
	}
}
