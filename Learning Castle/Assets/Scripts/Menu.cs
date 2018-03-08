using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public static int castle = 0;

	private Image rulesImage;
	private Text rulesText;
	private bool mouseOver = false;
	private float fadeTime = 5;

	void Start(){
		bool testCompleted = GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ().completedTests [castle - 1];
		string minigameRules;

		rulesText = GameObject.Find ("Rules").GetComponentInChildren<Text> ();
		rulesImage = GameObject.Find ("Rules").GetComponent<Image> ();	
		rulesText.color = rulesImage.color = Color.clear;

		// MiniGame button
		if (castle == 1)
			minigameRules = "Нажимайте на пузырьки, которые содержат число, соответствующее квадратному корню в задании";
		else {
			minigameRules = "Миниигра отсутствует";
			GameObject.Find ("MiniGame").GetComponentInChildren<Button> ().interactable = false;
			GameObject.Find ("MiniGame").GetComponentInChildren<Text> ().color = new Color (0.2f, 0.2f, 0.2f, 0.5f);
		}
		rulesText.text = minigameRules;

		// Test button
		if (testCompleted) {
			GameObject.Find ("Test").GetComponent<Button> ().interactable = false;
			GameObject.Find ("Test").GetComponentInChildren<Text> ().color = new Color (0.2f, 0.2f, 0.2f, 0.5f);
		}
	}

	void Update () {
		if (mouseOver) {
			rulesText.color = Color.Lerp (rulesText.color, Color.black, fadeTime * Time.deltaTime);
			rulesImage.color = Color.Lerp (rulesImage.color, new Color(1, 1, 1, 0.5f) , fadeTime * Time.deltaTime);
		} else {
			rulesText.color = Color.Lerp (rulesText.color, Color.clear, fadeTime * Time.deltaTime);
			rulesImage.color = Color.Lerp (rulesImage.color, Color.clear, fadeTime * Time.deltaTime);
		}
	}

	public void LoadGame(){		
		if (castle == 1) {
			SceneManager.LoadScene ("Bubbles");
			BubbleGame.numOfRounds = 10;
		} 
		// Other minigame loading
	}

	public void OnMouseOver()
	{
		mouseOver = true;
	}

	public void OnMouseExit()
	{
		mouseOver = false;
	}
}