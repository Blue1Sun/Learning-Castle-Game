using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Test : MonoBehaviour {

	public QType qtype;
	public enum QType {
		oneA,
		manyA,
		textA
	};

	private GameObject toggles;
	private GameObject longAnsw;

	// Use this for initialization
	void Start () {
		toggles = GameObject.Find ("Toggles");
		longAnsw = GameObject.Find ("LongAnswer");

		NextQuestion ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void NextQuestion() {

		Toggle answer1 = null;
		Toggle answer2 = null;
		Toggle answer3 = null;
		Toggle answer4 = null;

		if (qtype == QType.manyA) {
			toggles.SetActive(true);
			longAnsw.SetActive (false);

			findToggles (ref answer1, ref answer2, ref answer3, ref answer4);

			answer1.isOn = answer2.isOn = answer3.isOn = answer4.isOn = false;

			answer1.group = null;
			answer2.group = null;
			answer3.group = null;
			answer4.group = null;
		} 
		else if (qtype == QType.oneA) {
			toggles.SetActive(true);
			longAnsw.SetActive (false);
			ToggleGroup tg = GameObject.Find ("Toggles").GetComponent<ToggleGroup> ();

			findToggles (ref answer1, ref answer2, ref answer3, ref answer4);

			answer1.isOn = true;
			answer2.isOn = answer3.isOn = answer4.isOn = false;

			answer1.group = tg;
			answer2.group = tg;
			answer3.group = tg;
			answer4.group = tg;
		} 
		else {
			toggles.SetActive(false);
			longAnsw.SetActive (true);
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
