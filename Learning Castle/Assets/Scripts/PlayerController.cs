using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	
	private Animator charAnimation;

	void Start () {
		charAnimation = GetComponent<Animator> ();	

		if (Menu.castle != 0) {
			Vector2 lastCastlePos;

			if (Menu.castle % 3 == 1)
				lastCastlePos = GameObject.Find ("Castle").GetComponent<Transform> ().position;
			else
				lastCastlePos = GameObject.Find ("Castle (" + ((Menu.castle + 1) % 3 + 1) + ")").GetComponent<Transform> ().position;

			GetComponent<Transform> ().position = new Vector2 (lastCastlePos.x, lastCastlePos.y - 1);
		}		

		charAnimation.speed = 1.5f;
	}

	void FixedUpdate () {
		string gender = GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ().Gender;
		string cGen = "";
		if (gender.Equals("f"))
			cGen = "W";

		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
			charMoving ("WalkUp" + cGen, Vector2.up);
		} 
		else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
			charMoving ("WalkLeft" + cGen, Vector2.left);
		} 
		else if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
			charMoving ("WalkDown" + cGen, Vector2.down);
		} 
		else if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			charMoving ("WalkRight" + cGen, Vector2.right);
		} 
		else {
			charAnimation.Play ("Idle" + cGen);
		}
	}

	void charMoving (string animName, Vector2 movement){
		charAnimation.Play (animName);

		movement *= 0.1f;
		if (canPlayerMove (movement)) {		
			float walkSpeed = 60;
			GetComponent<Rigidbody2D> ().AddForce (movement * walkSpeed);
		}
	}

	bool canPlayerMove(Vector2 direction){
		bool flag = true;
		float distance = GetComponent<Transform>().position.z - Camera.main.transform.position.z;

		Vector3 leftdown = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distance));
		Vector3 rightup = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, distance));

		GameObject backGround = GameObject.Find ("BackGround");
		float ySpriteBorder = backGround.GetComponent<SpriteRenderer> ().sprite.bounds.extents.y * backGround.transform.localScale.y;
		float xSpriteBorder = backGround.GetComponent<SpriteRenderer> ().sprite.bounds.extents.x * backGround.transform.localScale.x;

		// If movement makes camera border move futher than BG border
		if ( (direction.x < 0 && leftdown.x <= -xSpriteBorder + 0.5f) || (direction.x > 0 && rightup.x >= xSpriteBorder - 0.5f) || 
			(direction.y < 0 && leftdown.y <= -ySpriteBorder + 0.5f) || (direction.y > 0 && rightup.y >= ySpriteBorder - 0.5f) )
			flag = false;

		return flag;
	}
	public void goToSubjects(){
		Menu.castle = 0;
		SceneManager.LoadScene ("Subjects");
	}
}