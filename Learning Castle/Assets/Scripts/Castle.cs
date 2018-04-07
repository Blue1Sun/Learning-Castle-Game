using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Castle : MonoBehaviour {

	public static bool[] hasMinigame = { true, true, true, true, false }; //FILLME: don't forget to fill it!!!

	[SerializeField]
	private Text text = null;
	[SerializeField]
	private string castleName = "NoName";
	[SerializeField]
	private Color nameColor = Color.white;
	[SerializeField]
	private Sprite completedCastle = null;
	[SerializeField]
	private int castleNum = 0;

	private bool mouseOver = false;

	void Start () {
		text.color = Color.clear; 
		text.text = castleName;	

		bool[] isCompleted = GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ().IsCompleted;
		if (isCompleted [castleNum - 1]) {
			this.GetComponent<SpriteRenderer> ().sprite = completedCastle;
		}
	}
		
	void Update () {
		if (mouseOver)
			rulesFading (nameColor);
		else
			rulesFading (Color.clear);
	}

	void rulesFading(Color after){
		float fadeTime = 5;
		text.color = Color.Lerp (text.color, after, fadeTime * Time.deltaTime);
	}

	public void OnMouseOver()
	{
		mouseOver = true;
	}

	public void OnMouseExit()
	{
		mouseOver = false;
	}

	public void OnMouseClick()
	{
		Menu.castle = castleNum;
		SceneManager.LoadScene ("Menu");
	}

}