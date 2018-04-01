using System.Collections;
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.UI;

public class EngGuitarGame : MonoBehaviour {

	public int numOfRounds = 10;

	private class EngRusDict
	{	

		private string engWord;
		private string[] rusWords = new string[4];

		private bool isUsed;
		private int correctAnswer;

		public EngRusDict(string engWord, string rusWord1, string rusWord2, string rusWord3, string rusWord4, int correctAnswer){
			this.engWord = engWord;

			rusWords[0] = rusWord1;
			rusWords[1] = rusWord2;
			rusWords[2] = rusWord3;
			rusWords[3] = rusWord4;

			isUsed = false;
			this.correctAnswer = correctAnswer;
		}

		public void setIsUsed(bool isUsed){
			this.isUsed = isUsed;
		}
		public bool getIsUsed(){
			return isUsed;
		}

		public int getCorrectAnswer(){
			return correctAnswer;
		}

		public string getEngWord(){
			return engWord;
		}

		public string getRusWord(int index){
			return rusWords[index];
		}
	};

	private int curRound;
	private int score;

	private GameObject window;

	private EngRusDict[] engRusDict;
	
	// Use this for initialization
	void Start () {
		curRound = 0;
		score = 0;

		DictionaryCreation ();

		window = GameObject.Find ("Window");
		window.SetActive (false);

		NextRound ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow)) {
			Destroy (GameObject.Find ("Line"));
			AnimateArrow ("LeftArrow");
		} 
		else if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
			AnimateArrow ("DownArrow");
		} 
		else if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) {
			AnimateArrow ("UpArrow");
		} 
		else if (Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)) {
			AnimateArrow ("RightArrow");
		}

		if (GameObject.Find ("Line") && GameObject.Find ("Line").GetComponent<Transform> ().position.y < -3.5f)
			NextRound ();
	}

	void DictionaryCreation(){
		engRusDict = new EngRusDict[30];
		engRusDict [0] = new EngRusDict ("anxious", "тревожный", "неприятный", "свирепый", "граненый", 0);
		engRusDict [1] = new EngRusDict ("plaque", "письмо", "сказочный мир", "божья коровка", "бляшка", 3);
		engRusDict [2] = new EngRusDict ("humbly", "обычно", "смиренно", "дополнительно", "чудесно", 1);
		engRusDict [3] = new EngRusDict ("infuriate", "ослаблять", "взбесить", "растворяться", "помогать", 1);
		engRusDict [4] = new EngRusDict ("palate", "палата", "храм", "вкус", "склонность", 2);

		engRusDict [5] = new EngRusDict ("quicksilver", "склонность", "краткость", "ртуть", "гидрокостюм", 2);
		engRusDict [6] = new EngRusDict ("mist", "тайна", "бродяга", "туман", "финиш", 2);
		engRusDict [7] = new EngRusDict ("evidence", "доказательства", "принуждение", "потомок", "неловкость", 0);
		engRusDict [8] = new EngRusDict ("brave", "групповой", "храбрый", "растянутый", "аккуратный", 1);
		engRusDict [9] = new EngRusDict ("civil", "гражданский", "пригородный", "губчатый", "средний", 0);

		engRusDict [10] = new EngRusDict ("twitch", "спрашивать", "олицетворять", "дергаться", "быть", 2);
		engRusDict [11] = new EngRusDict ("undergo", "подвергаться", "кричать", "медлить", "прерывать", 0);
		engRusDict [12] = new EngRusDict ("foolish", "влажный", "хвастливый", "промежуточный", "глупый", 3);
		engRusDict [13] = new EngRusDict ("tool", "весло", "мешанина", "правота", "инструмент", 3);
		engRusDict [14] = new EngRusDict ("curious", "любопытный", "серьезный", "безмолвный", "ироничный", 0);

		engRusDict [15] = new EngRusDict ("spoil", "согласовываться", "портить", "сокращаться", "суетиться", 1);
		engRusDict [16] = new EngRusDict ("lightning", "обстоятельство", "вырождение", "молния", "дикарь", 2);
		engRusDict [17] = new EngRusDict ("announce", "слышать", "переусердствовать", "объявлять", "оставаться", 2);
		engRusDict [18] = new EngRusDict ("smugly", "смугло", "предварительно", "самодовольно", "презрительно", 2);
		engRusDict [19] = new EngRusDict ("suspicious", "тупой", "лимонный", "жестокий", "подозрительный", 3);

		engRusDict [20] = new EngRusDict ("glass", "фанера", "ежевика", "шум", "стекло", 3);
		engRusDict [21] = new EngRusDict ("difference", "перекресток", "злоупотребление", "пучок", "разница", 3);
		engRusDict [22] = new EngRusDict ("temporal", "остроумный", "временный", "остаточный", "перезрелый", 1);
		engRusDict [23] = new EngRusDict ("harmlessly", "безвредно", "обычно", "грандиозно", "легко", 0);
		engRusDict [24] = new EngRusDict ("curiously", "неумолимо", "предусмотрительно", "странно", "ярко", 2);

		engRusDict [25] = new EngRusDict ("memento", "отклонение", "напоминание", "греховность", "момент", 1);
		engRusDict [26] = new EngRusDict ("viscous", "вязкий", "неудержимый", "язвительный", "гневный", 0);
		engRusDict [27] = new EngRusDict ("pitiable", "трупный", "многословный", "жалкий", "громоздкий", 2);
		engRusDict [28] = new EngRusDict ("haul", "тащить", "скрываться", "представлять", "кричать", 0);
		engRusDict [29] = new EngRusDict ("daze", "бухта", "двор", "ловкость", "изумление", 3);

	}

	void loadInfo(){
		int randNum = -1;
		do {
			randNum = Random.Range (0, engRusDict.Length - 1);
		} while (engRusDict [randNum].getIsUsed() == true);
		engRusDict [randNum].setIsUsed (true);

		EngRusDict randWord = engRusDict [randNum];

		GameObject.Find ("Rounds").GetComponent<Text> ().text = curRound + " / " + numOfRounds;

		GameObject.Find ("EnglishWord").GetComponent<Text> ().text = randWord.getEngWord ();

		GameObject.Find ("LeftPos").GetComponentInChildren<Text> ().text = randWord.getRusWord(0);
		GameObject.Find ("DownPos").GetComponentInChildren<Text> ().text = randWord.getRusWord(1);
		GameObject.Find ("UpPos").GetComponentInChildren<Text> ().text = randWord.getRusWord(2);
		GameObject.Find ("RightPos").GetComponentInChildren<Text> ().text = randWord.getRusWord(3);


		GameObject.Find ("LeftArrow").tag = GameObject.Find ("DownArrow").tag = GameObject.Find ("UpArrow").tag = GameObject.Find ("RightArrow").tag = "Untagged";

		switch (engRusDict [randNum].getCorrectAnswer()) {
		case 0: 
			GameObject.Find ("LeftArrow").tag = "CorrectArrow";
			break;
		case 1: 
			GameObject.Find ("DownArrow").tag = "CorrectArrow";
			break;
		case 2: 
			GameObject.Find ("UpArrow").tag = "CorrectArrow";
			break;
		case 3: 
			GameObject.Find ("RightArrow").tag = "CorrectArrow";
			break;
		default:
			Debug.LogError ("Wrong index of correct answer");
			break;
		}
	}

	void AnimateArrow(string arrow){
		var arrowsAnimator = GameObject.Find ("Arrows").GetComponentsInChildren<Animator> ();
		foreach (Animator arrAnim in arrowsAnimator) {
			arrAnim.Rebind ();
			arrAnim.Play ("IdleArrow");
		}

		GameObject.Find (arrow).GetComponent<Animator> ().Play ("Flying");

		CheckingAnswer (arrow);
	}

	void CheckingAnswer(string answer){

		if (GameObject.Find (answer).tag == "CorrectArrow"){
			score++;
			GameObject.Find ("Score").GetComponent<Text> ().text = score.ToString();
		}

		NextRound ();
	}

	void NextRound(){
		curRound++;

		GameObject Line = GameObject.Find ("Line");
		Destroy (Line);

		if (curRound <= numOfRounds) {
			GameObject newLine = Instantiate (Line, new Vector3 (0, 3.7f, 0), Quaternion.identity) as GameObject;	
			newLine.name = "Line";

			loadInfo ();
		} 
		else {
			GameEnd ();
		}
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
		if (score > GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ().minigameRecord [Menu.castle - 1])
			GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ().minigameRecord [Menu.castle - 1] = score;
		
		GameObject.Find("ResultMessage").GetComponent<Text>().text = resultMessage;
	}
}
