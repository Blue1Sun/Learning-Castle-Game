	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float WalkSpeed = 60;

	private Animator CharAnimation;
	private Rigidbody2D CharBody;

	void Start () {
		CharAnimation = GetComponent<Animator> ();	
		CharBody = GetComponent<Rigidbody2D> ();

		CharAnimation.speed = 1.5f;
	}

	void FixedUpdate () {
		PlayerData.Gender gender = GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ().gender;
		string cGen = "";
		if (gender == PlayerData.Gender.Woman)
			cGen = "W";

		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
			CharAnimation.Play ("WalkUp"+cGen);
			if (canPlayerMove("up")){
				Vector2 Movement = new Vector2 (0, 0.1f);
				CharBody.AddForce (Movement * WalkSpeed);
			}
		} 
		else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
			CharAnimation.Play ("WalkLeft"+cGen);
			if (canPlayerMove ("left")) {
				Vector2 Movement = new Vector2 (-0.1f, 0);
				CharBody.AddForce (Movement * WalkSpeed);
			}
		} 
		else if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
			CharAnimation.Play ("WalkDown"+cGen);
			if (canPlayerMove ("down")) {
				Vector2 Movement = new Vector2 (0, -0.1f);
				CharBody.AddForce (Movement * WalkSpeed);
			}
		} 
		else if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			CharAnimation.Play ("WalkRight"+cGen);
			if (canPlayerMove ("right")) {
				Vector2 Movement = new Vector2 (0.1f, 0);
				CharBody.AddForce (Movement * WalkSpeed);
			}
		} 
		else 
		{
			CharAnimation.Play ("Idle"+cGen);
		}
	}

	bool canPlayerMove(string direction)
	{
		bool flag = true;

		float distance = this.transform.position.z - Camera.main.transform.position.z;

		Vector3 leftdown = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
		Vector3 rightup = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, distance));

		float ySprite = GameObject.Find ("BackGround").GetComponent<SpriteRenderer>().sprite.bounds.extents.y * GameObject.Find ("BackGround").transform.localScale.y;
		float xSprite =  GameObject.Find ("BackGround").GetComponent<SpriteRenderer>().sprite.bounds.extents.x * GameObject.Find ("BackGround").transform.localScale.x;

		if (direction == "left" && leftdown.x <= -xSprite + 0.5f)
			flag = false;
		else if (direction == "right" && rightup.x >= xSprite - 0.5f)
			flag = false;
		else if (direction == "down" && leftdown.y <= -ySprite + 0.5f)
			flag = false;
		else if (direction == "up" && rightup.y >= ySprite - 0.5f)
			flag = false;

		Debug.Log (flag);

		return flag;
	}
}