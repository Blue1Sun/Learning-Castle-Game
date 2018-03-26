using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Castle : MonoBehaviour {

	public static bool[] hasMinigame = { true, true, false }; //TODO: don't forget to fill it!!!

	public Text text;
	public string castleName = "NoName";
	public Color nameColor = Color.white;
	public Sprite completedCastle;
	public int castleNum = 0;

	private float fadeTime = 5;
	private bool mouseOver = false;

	void Start () {
		text.color = Color.clear; 
		text.text = castleName;	

		bool[] isCompleted = GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ().isCompleted;
		if (isCompleted [castleNum - 1]) {
			this.GetComponent<SpriteRenderer> ().sprite = completedCastle;
		}
	}
		
	void Update () {
		if (mouseOver)
			text.color = Color.Lerp (text.color, nameColor, fadeTime * Time.deltaTime);
		else
			text.color = Color.Lerp (text.color, Color.clear, fadeTime * Time.deltaTime);
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