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

	private int indexDot;

	// Use this for initialization
	void Start () {
		playerCoord = new List<Coordinate>{ };
		moving = false;
		movingToGraph = false;
		drawingGraph = false;


		zeroPos = GameObject.Find ("MiniGame").GetComponent<Transform> ().position;
		minigame = GameObject.Find ("MiniGame").GetComponent<Graphs> ();
	
		indexDot = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (movingToGraph) {
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
			float diffX = playerCoord [indexDot + 1].X - playerCoord [indexDot].X;
			float diffY = playerCoord [indexDot + 1].Y - playerCoord [indexDot].Y;

			float posX = this.transform.position.x + diffX;
			float posY = this.transform.position.y + diffY;

			this.transform.position = new Vector3(posX, posY, 0);

			GameObject dot = GameObject.Instantiate (dotPrefab, new Vector3 (posX, posY, 0), Quaternion.identity) as GameObject;
			dot.transform.parent = GameObject.Find ("Dots").GetComponent<Transform>();

			indexDot++;

		}
		if (indexDot >= playerCoord.Count-1 && moving) {
			minigame.NextRound ();
		}
	}

	//Starting with moving
	void OnTriggerStay2D(Collider2D collider) {
		if (collider.name == "Start" && moving == true) {
			this.transform.position += Vector3.right * 2 * Time.deltaTime;
		}
	}

	//Flying to start of graph
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.name == "Finish" && !drawingGraph) {
			movingToGraph = true;

			this.GetComponent<Rigidbody2D> ().simulated = false;
			this.GetComponent<CapsuleCollider2D> ().enabled = false;
		}
	}
}
