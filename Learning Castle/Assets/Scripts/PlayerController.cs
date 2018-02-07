	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public class PlayerInfo
	{
		public Gender gender;
		public bool[] isCompleted;
	}

	public float WalkSpeed = 60;
	public Gender gender;

	public enum Gender {Woman, Man};
	private Animator CharAnimation;
	private Rigidbody2D CharBody;

	void Start () {
		CharAnimation = GetComponent<Animator> ();	
		CharBody = GetComponent<Rigidbody2D> ();	

		CharAnimation.speed = 1.5f;
	}

	void FixedUpdate () {
		
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
			CharAnimation.Play ("WalkUp");
			Vector2 Movement = new Vector2 (0, 0.1f);
			CharBody.AddForce (Movement * WalkSpeed);
		} 
		else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
			CharAnimation.Play ("WalkLeft");
			Vector2 Movement = new Vector2 (-0.1f, 0);
			CharBody.AddForce (Movement * WalkSpeed);
		} 
		else if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
			CharAnimation.Play ("WalkDown");
			Vector2 Movement = new Vector2 (0, -0.1f);
			CharBody.AddForce (Movement * WalkSpeed);
		} 
		else if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			CharAnimation.Play ("WalkRight");
			Vector2 Movement = new Vector2 (0.1f, 0);
			CharBody.AddForce (Movement * WalkSpeed);
		} 
		else 
		{
			CharAnimation.Play ("Idle");
		}

	}
}