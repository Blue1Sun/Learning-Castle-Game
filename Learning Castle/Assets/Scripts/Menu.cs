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

	void Start(){
		PlayerData playerData = GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ();
		bool testCompleted = playerData.CompletedTests [castle - 1];

		GameObject rules = GameObject.Find ("Rules");
		rulesText = rules.GetComponentInChildren<Text> ();
		rulesImage = rules.GetComponent<Image> ();	

		rulesText.color = rulesImage.color = Color.clear;

		setMinigameRule (playerData);

		// Test button
		if (testCompleted) {
			GameObject test = GameObject.Find ("Test");
			test.GetComponent<Button> ().interactable = false;
			test.GetComponentInChildren<Text> ().color = new Color (0.2f, 0.2f, 0.2f, 0.5f);
		}

		if (playerData.CompletedTests [castle - 1] && (playerData.MinigameRecord [castle - 1] != -100 || !Castle.hasMinigame [castle - 1]))
			playerData.IsCompleted [castle - 1] = true;
	}

	void setMinigameRule(PlayerData playerData){
		string[] minigameRules = new string[]{ "Нажимайте на пузырьки, которые содержат число, соответствующее квадратному корню в задании.", 
			"Нажимайте на облака, которые содержат два корня x1 и x2, соответствующие ответам на квадратное уравнение.", 
			"Выберете уравнение, соответствующее представленному справа графику. Лыжник полетит по выбранному вами уравнению.", 
			"Нажимайте на стрелки, под которыми написан перевод заданного слова. Управление с помощью WASD или стрелочки.",
			"Проходите через стену, на которой написан артикль, подходящий пропуску в предложении. Управление с помощью AWD или ←↑→. Вверх - ускорение."};
		string minigameRule = "";

		if (castle <= minigameRules.Length) {
			minigameRule = minigameRules [Menu.castle - 1];
		} 
		else {
			minigameRule = "Миниигра отсутствует.";

			GameObject.Find ("MiniGame").GetComponentInChildren<Button> ().interactable = false;
			GameObject.Find ("Button").GetComponentInChildren<Text> ().color = new Color (0.2f, 0.2f, 0.2f, 0.5f);

			Debug.LogWarning ("No rule for " +  castle + " minigame");
		}
		if (playerData.MinigameRecord [castle - 1] > -100)
			minigameRule += " <b>Ваш текущий рекорд: " + playerData.MinigameRecord [castle - 1] + "</b>";
		rulesText.text = minigameRule;
	}

	void Update () {
		if (mouseOver) {
			rulesFading (Color.black, new Color (1, 1, 1, 0.5f));
		} else {
			rulesFading (Color.clear, Color.clear);
		}
	}

	void rulesFading(Color text, Color panel){
		float fadeTime = 5;
		rulesText.color = Color.Lerp (rulesText.color, text, fadeTime * Time.deltaTime);
		rulesImage.color = Color.Lerp (rulesImage.color, panel, fadeTime * Time.deltaTime);
	}

	public void LoadGame(){		

		string[] gameNames = new string[]{ "Bubbles", "Clouds", "Graphs", "EngGuitarHero", "Articles" };

		if (castle <= gameNames.Length) {			
			SceneManager.LoadScene (gameNames [Menu.castle - 1]);
		} 
		else {
			Debug.LogWarning ("No minigame for " + castle + " castle");
		}	
	}

	public void ExitMenu(){
		if (castle < 4)
			SceneManager.LoadScene ("Map");
		else
			SceneManager.LoadScene ("Map 1");
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