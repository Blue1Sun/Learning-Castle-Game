using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float walkSpeed = 60;

	private Animator charAnimation;
	private Rigidbody2D charBody;

	void Start () {
		charAnimation = GetComponent<Animator> ();	
		charBody = GetComponent<Rigidbody2D> ();

		charAnimation.speed = 1.5f;
	}

	void FixedUpdate () {
		PlayerData.Gender gender = GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ().gender;
		string cGen = "";
		if (gender == PlayerData.Gender.Woman)
			cGen = "W";

		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
			charMoving ("WalkUp" + cGen, new Vector2 (0, 0.1f));
		} 
		else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
			charMoving ("WalkLeft" + cGen, new Vector2 (-0.1f, 0));
		} 
		else if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
			charMoving ("WalkDown" + cGen, new Vector2 (0, -0.1f));
		} 
		else if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			charMoving ("WalkRight" + cGen, new Vector2 (0.1f, 0));
		} 
		else {
			charAnimation.Play ("Idle" + cGen);
		}
	}

	void charMoving (string animName, Vector2 movement){
		charAnimation.Play (animName);

		if (canPlayerMove (movement)) {				
			charBody.AddForce (movement * walkSpeed);
		}
	}

	bool canPlayerMove(Vector2 direction)
	{
		bool flag = true;
		float distance = this.transform.position.z - Camera.main.transform.position.z;

		Vector3 leftdown = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distance));
		Vector3 rightup = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, distance));

		float ySpriteBorder = GameObject.Find ("BackGround").GetComponent<SpriteRenderer> ().sprite.bounds.extents.y * GameObject.Find ("BackGround").transform.localScale.y;
		float xSpriteBorder = GameObject.Find ("BackGround").GetComponent<SpriteRenderer> ().sprite.bounds.extents.x * GameObject.Find ("BackGround").transform.localScale.x;

		// If movement makes camera border move futher than BG border
		if ( (direction.x < 0 && leftdown.x <= -xSpriteBorder + 0.5f) || (direction.x > 0 && rightup.x >= xSpriteBorder - 0.5f) || (direction.y < 0 && leftdown.y <= -ySpriteBorder + 0.5f) || (direction.y > 0 && rightup.y >= ySpriteBorder - 0.5f) )
			flag = false;

		return flag;
	}
}