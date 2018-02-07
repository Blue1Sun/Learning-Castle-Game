using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public static int castle = 0;

	private Image rulesImage;
	private Text rulesText;
	private bool MouseOver = false;
	private float fadeTime = 5;
	private string MinigameRules;

	void Start(){
		rulesText = GameObject.Find ("Rules").GetComponentInChildren<Text>();
		rulesImage = GameObject.Find ("Rules").GetComponent<Image>();	
		rulesText.color = rulesImage.color = Color.clear;

		if (castle == 1)
			MinigameRules = "Нажимайте на пузырьки, которые содержат число, соответствующее квадратному корню в задании";

		rulesText.text = MinigameRules;
	}

	void Update () {
		if (MouseOver) {
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
		else
			SceneManager.LoadScene("Map");
	}

	public void OnMouseOver()
	{
		MouseOver = true;
	}

	public void OnMouseExit()
	{
		MouseOver = false;
	}
}