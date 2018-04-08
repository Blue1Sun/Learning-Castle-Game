using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperController : MonoBehaviour {

	public class Coordinate{
		private float x; 
		private float y;

		public float X{
			get	{ return x; }
			set	{ x = value; }
		}

		public float Y{
			get	{ return y; }
			set	{ y = value; }
		}

		public Coordinate(float x, float y)
		{
			this.x = x;
			this.y = y;
		}
	}

	public List<Coordinate> playerCoord;
	
	public GameObject dotPrefab;
	public bool moving;

	private Graphs minigame;

	private bool movingToGraph;
	private bool drawingGraph;
	private Vector2 zeroPos;

	private float x1;
	private float x1Max;

	private int indexDot;

	// Use this for initialization
	void Start () {
		playerCoord = new List<Coordinate>{ };
		moving = false;
		movingToGraph = false;
		drawingGraph = false;


		zeroPos = GameObject.Find ("MiniGame").GetComponent<Transform> ().position;
		minigame = GameObject.Find ("MiniGame").GetComponent<Graphs> ();

		x1 = 0;		
		x1Max = 0;
		indexDot = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (movingToGraph) {
			// Клетка == 0.5f
			// Центр: 4.7f x ; -0.26f y
			// Уголок: 1.2f x ; 3.23f y
			float step = 4 * Time.deltaTime;
			float startX = playerCoord [0].X;
			float startY = playerCoord [0].Y;

			startX = zeroPos.x + startX;
			startY = zeroPos.y + startY;
			Vector2 startPos = new Vector3 (startX, startY, 0);

			transform.position = Vector3.MoveTowards(transform.position, startPos, step);
			if (transform.position.x == startPos.x && transform.position.y == startPos.y) {
				movingToGraph = false;
				drawingGraph = true;
			}
		}
		if (drawingGraph && indexDot < playerCoord.Count-1)
		{
			//for (float x1 = -2; x1 < 2; x1 +=  0.01f) {
			//float y1 = x1 * x1;

			//float x2 = x1 + 0.05f;
			//float y2 = x2 * x2;

			//float diffX = x2 - x1;
			//float diffY = y2 - y1;

			//float posX = this.transform.position.x + diffX;
			//float posY = this.transform.position.y + diffY;

			//print (x1 + " " + y1 + " " + x2 + " " + y2);

			float diffX = playerCoord [indexDot + 1].X - playerCoord [indexDot].X;
			float diffY = playerCoord [indexDot + 1].Y - playerCoord [indexDot].Y;

			float posX = this.transform.position.x + diffX;
			float posY = this.transform.position.y + diffY;

			this.transform.position = new Vector3(posX, posY, 0);

			GameObject dot = GameObject.Instantiate (dotPrefab, new Vector3 (posX, posY, 0), Quaternion.identity) as GameObject;
			dot.transform.parent = GameObject.Find ("Dots").GetComponent<Transform>();

			indexDot++;

			//x1 += 0.05f;
		}
		if (indexDot >= playerCoord.Count-1 && moving) {
			minigame.NextRound ();
		}
	}

	//Starting with moving
	void OnTriggerStay2D(Collider2D collider) {
		Debug.Log ("OnTriggerStay2D");
		if (collider.name == "Start" && moving == true) {
			this.transform.position += Vector3.right * 2 * Time.deltaTime;
		}
	}

	//Flying to start of graph
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.name == "Finish" && !drawingGraph) {
			//JumperGraph ();
			movingToGraph = true;
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
