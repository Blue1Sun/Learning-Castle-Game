using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour {

	[SerializeField]
	private AudioClip pop = null;

	private BubbleGame minigame;

	private int x;
	private bool isCorrect = false;
	private string x1x2 = "";

	public int X {
		get	{ return x;	}
		set	{ x = value; }
	}

	public bool IsCorrect {
		get	{ return isCorrect;	}
		set	{ isCorrect = value; }
	}

	public string X1X2 {
		get	{ return x1x2;	}
		set	{ x1x2 = value; }
	}

	void Start () {
		this.GetComponentInChildren<Text>().text = x.ToString ();
		if (x1x2 != "")
			this.GetComponentInChildren<Text>().text = x1x2;

		minigame = GameObject.Find ("MiniGame").GetComponent<BubbleGame> ();
	}

	void Update () {
		if (this.isCorrect && this.transform.position.y < -5) {
			minigame.Score--;
			DestroyingBubbles ();
			Destroy (gameObject);
			ScoreUpdate ();
		}
	}

	void OnMouseDown () {
		AudioSource.PlayClipAtPoint (pop, Camera.main.transform.position);
		if (isCorrect) {
			minigame.Score++;
			DestroyingBubbles ();
		} else {
			minigame.Score--;
		}
		Destroy (gameObject);
		ScoreUpdate ();
	}

	void ScoreUpdate () {
		Text scoreText = GameObject.Find ("Score").GetComponent<Text>();
		scoreText.text = minigame.Score.ToString ();
	}

	void DestroyingBubbles () {
		GameObject[] wrongBubbles = GameObject.FindGameObjectsWithTag ("WrongBubble");
		for (int i = 0; i < wrongBubbles.Length; i++)
			Destroy (wrongBubbles[i]);
	}
}
