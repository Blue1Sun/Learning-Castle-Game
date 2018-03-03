using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Test : MonoBehaviour {

	public class Question
	{
		public int type; //1 - oneA, 2 - manyA, 3 - textA
		public string question;
		public string[] answers;

		public Question(int type, string question, string[] answers)
		{
			this.type = type;
			this.question = question;
			this.answers = answers;
		}
	};

	public QType qtype; //сделать так чтобы брался тип след/пред вопроса
	public int numOfQuestions = 10;
	public enum QType {
		oneA,
		manyA,
		textA
	};

	private GameObject toggles;
	private GameObject longAnsw;
	private int curQuestion;
	private string[] playerAnswers;

	// Use this for initialization
	void Start () {
		Question[] questions; //массив классов - одна тема
		curQuestion = 0;
		toggles = GameObject.Find ("Toggles");
		longAnsw = GameObject.Find ("LongAnswer");

		NextQuestion ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PrevQuestion(){
		curQuestion--;
		changeLayout (qtype);
	}

	public void NextQuestion() {
		curQuestion++;
		if (curQuestion > numOfQuestions)
			SceneManager.LoadScene ("Menu");
		else
			changeLayout (qtype);
	}

	void changeLayout(QType qtype)
	{
		Toggle answer1 = null;
		Toggle answer2 = null;
		Toggle answer3 = null;
		Toggle answer4 = null;

		if (qtype == QType.manyA || qtype == QType.oneA) {
			toggles.SetActive (true);
			longAnsw.SetActive (false);
			findToggles (ref answer1, ref answer2, ref answer3, ref answer4);
		} else {
			toggles.SetActive(false);
			longAnsw.SetActive (true);
		}

		if (qtype == QType.manyA) {
			answer1.isOn = answer2.isOn = answer3.isOn = answer4.isOn = false;

			answer1.group = null;
			answer2.group = null;
			answer3.group = null;
			answer4.group = null;
		} 
		else if (qtype == QType.oneA) {
			ToggleGroup tg = GameObject.Find ("Toggles").GetComponent<ToggleGroup> ();

			answer1.isOn = true;
			answer2.isOn = answer3.isOn = answer4.isOn = false;

			answer1.group = tg;
			answer2.group = tg;
			answer3.group = tg;
			answer4.group = tg;
		} 

		GameObject bBack = GameObject.Find ("Back");
		GameObject bNext = GameObject.Find ("Next");

		if (curQuestion == 1) {			
			bBack.GetComponent<Button> ().interactable = false;
			bBack.GetComponentInChildren<Text> ().color = new Color (0.2f, 0.2f, 0.2f, 0.5f);
		} else {
			bBack.GetComponent<Button> ().interactable = true;
			bBack.GetComponentInChildren<Text> ().color = new Color (0.2f, 0.2f, 0.2f, 1f);
		}

		if (curQuestion == numOfQuestions) {
			bNext.GetComponentInChildren<Text> ().text = "Завершить";
		} else {
			bNext.GetComponentInChildren<Text> ().text = "Далее";
		}
	}

	void findToggles(ref Toggle a1, ref Toggle a2, ref Toggle a3, ref Toggle a4)
	{
		a1 = GameObject.Find ("1Answer").GetComponent<Toggle> ();
		a2 = GameObject.Find ("2Answer").GetComponent<Toggle> ();
		a3 = GameObject.Find ("3Answer").GetComponent<Toggle> ();
		a4 = GameObject.Find ("4Answer").GetComponent<Toggle> ();
	}

}
