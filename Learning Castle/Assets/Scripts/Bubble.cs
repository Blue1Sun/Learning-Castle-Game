using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour {

	public AudioClip pop;

	public int x;
	public bool isCorrect = false;
	public string x1x2 = "";

	void Start () {
		this.GetComponentInChildren<Text>().text = x.ToString ();
		if (x1x2 != "")
			this.GetComponentInChildren<Text>().text = x1x2;
	}

	void Update () {
		if (this.isCorrect && this.transform.position.y < -5) {
			BubbleGame.score--;
			DestroyingBubbles ();
			Destroy (gameObject);
			ScoreUpdate ();
		}
	}

	void OnMouseDown () {
		AudioSource.PlayClipAtPoint (pop, Camera.main.transform.position);
		if (isCorrect) {
			BubbleGame.score++;
			DestroyingBubbles ();
		} else {
			BubbleGame.score--;
		}
		Destroy (gameObject);
		ScoreUpdate ();
	}

	void ScoreUpdate () {
		Text scoreText = GameObject.Find ("Score").GetComponent<Text>();
		scoreText.text = BubbleGame.score.ToString ();
	}

	void DestroyingBubbles () {
		GameObject[] wrongBubbles = GameObject.FindGameObjectsWithTag ("WrongBubble");
		for (int i = 0; i < wrongBubbles.Length; i++)
			Destroy (wrongBubbles[i]);
	}
}
