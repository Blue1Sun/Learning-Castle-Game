using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphs : MonoBehaviour {

	public int numOfDots;
	public float frequency = 0.2f;
	//public float force;

	public GraphType graphType;
	public enum GraphType {Parabola, Linear, Sqrt};


	void Start() {
		
	}

	// Update is called once per frame
	void Update () {
		if (graphType == GraphType.Parabola)
			ParabolaGraph ();
		if (graphType == GraphType.Linear)
			LinearGraph ();
		if (graphType == GraphType.Sqrt)
			SqrtGraph ();		
	}

	void ParabolaGraph(){
		LineRenderer lineRenderer = this.GetComponent<LineRenderer> ();

		lineRenderer.numPositions = numOfDots;
		float x = (-numOfDots / 2 + 0.5f) * frequency;
		for (int index = 0; index < numOfDots; index++) {
			float y = x * x;
			lineRenderer.SetPosition (index, new Vector3(x, y, 0));
			x += frequency;
		}
	}

	void LinearGraph(){
		LineRenderer lineRenderer = this.GetComponent<LineRenderer> ();
		lineRenderer.numPositions = numOfDots;
		int index = 0;
		for (float x = -(numOfDots / 2) * frequency; x < (numOfDots / 2) * frequency && index < numOfDots; x += frequency) {
			float y = x;
			lineRenderer.SetPosition (index, new Vector3 (x, y, 0));
			index++;
		}
	}

	void SqrtGraph(){
		LineRenderer lineRenderer = this.GetComponent<LineRenderer> ();

		lineRenderer.numPositions = numOfDots;
		int index = 0;
		for (float x = 0; x < numOfDots * frequency && index < numOfDots; x += frequency) {
			float y = Mathf.Sqrt(x);
			lineRenderer.SetPosition (index, new Vector3 (x, y, 0));
			index++;
		}
	}

	/*public void LaunchJumper(){
		GameObject.Find ("Jumper").GetComponent<Rigidbody2D>().velocity += Vector2.right * force;
	}*/
					
}
