using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperController : MonoBehaviour {
	
	public GameObject dotPrefab;

	private bool jumperJumping;

	private float x1;
	private float x1Max;

	// Use this for initialization
	void Start () {
		jumperJumping = false;
		x1 = 0;		
		x1Max = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (jumperJumping && x1 < x1Max) {
			//for (float x1 = -2; x1 < 2; x1 +=  0.01f) {
			float y1 = x1 * x1;

			float x2 = x1 + 0.05f;
			float y2 = x2 * x2;

			float diffX = x2 - x1;
			float diffY = y2 - y1;

			float posX = this.transform.position.x + diffX;
			float posY = this.transform.position.y + diffY;

			this.transform.position = new Vector3(posX, posY, 0);

			GameObject dot = GameObject.Instantiate (dotPrefab, new Vector3 (posX, posY, 0), Quaternion.identity) as GameObject;
			dot.transform.parent = GameObject.Find ("Dots").GetComponent<Transform>();

			x1 += 0.05f;

		}
	}

	void OnTriggerStay2D(Collider2D collider) {
		if (collider.name == "Start") {
			this.transform.position += Vector3.right * 2 * Time.deltaTime;
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.name == "Finish" && !jumperJumping) {
			//JumperGraph ();
			jumperJumping = true;
			x1 = -2; //TODO TAKE GRAPH FROM USER
			x1Max = 2;

			this.GetComponent<Rigidbody2D> ().simulated = false;
			this.GetComponent<CapsuleCollider2D> ().enabled = false;
		}
	}

	/*void JumperGraph () {
		float posY, posX;
		posX = this.transform.position.x;
		posY = this.transform.position.y;

		for (float x1 = -2; x1 < 2; x1 +=  0.1f) {
			float y1 = x1 * x1;

			float x2 = x1 + 0.1f;
			float y2 = x2 * x2;

			float diffX = x2 - x1;
			float diffY = y2 - y1;

			posX += diffX;
			posY += diffY;
					}
	}*/
}
