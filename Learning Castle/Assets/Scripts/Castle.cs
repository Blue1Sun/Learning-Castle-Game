using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Castle : MonoBehaviour {

	public Text text;
	public string myString = "NoName";
	public Color myColor = Color.white;
	public bool isCompleted = false;
	public Sprite completed;
	public int castleNum = 0;

	private float fadeTime = 5;
	private bool MouseOver = false;

	void Start () {
		text.color = Color.clear;
		text.text = myString;	
		bool[] isCompleted = GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ().isCompleted;
		if (isCompleted[castleNum-1]) {
			this.GetComponent<SpriteRenderer>().sprite = completed;
		}
	}
		
	void Update () {
		if (MouseOver)
			text.color = Color.Lerp (text.color, myColor, fadeTime * Time.deltaTime);
		else
			text.color = Color.Lerp (text.color, Color.clear, fadeTime * Time.deltaTime);
	}

	public void OnMouseOver()
	{
		MouseOver = true;
	}

	public void OnMouseExit()
	{
		MouseOver = false;
	}

	public void OnMouseClick()
	{
		this.isCompleted = true;
		Menu.castle = castleNum;
		SceneManager.LoadScene ("Menu");
	}

}